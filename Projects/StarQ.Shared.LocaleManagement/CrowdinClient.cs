using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace LocaleManagement
{
    internal sealed class CrowdinClient
    {
        private readonly HttpClient httpClient;
        private readonly string projectId;

        public CrowdinClient(string apiToken, string projectId)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            this.projectId = projectId;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.crowdin.com/api/v2/"),
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                apiToken
            );
        }

        public async Task<bool> BuildAndDownload()
        {
            try
            {
                var buildId = await StartBuild();

                await CheckUntilFinished(buildId);

                var downloadUrl = await GetDownloadUrl(buildId);
                await DownloadFile(downloadUrl);
                return true;
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }

        private async Task<string> StartBuild()
        {
            using HttpResponseMessage response = await httpClient.PostAsync(
                $"projects/{projectId}/bundles/1/exports",
                content: null
            );

            var body = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            using JsonDocument doc = JsonDocument.Parse(body);
            if (!doc.RootElement.TryGetProperty("data", out var data))
                throw new InvalidOperationException($"Unexpected Crowdin response: {body}");

            JsonElement idNum = data.GetProperty("identifier");
            return idNum.ValueKind == JsonValueKind.String
                ? idNum.GetString()!
                : idNum.GetRawText();
        }

        private async Task CheckUntilFinished(string buildId)
        {
            while (true)
            {
                using var response = await httpClient.GetAsync(
                    $"projects/{projectId}/bundles/1/exports/{buildId}"
                );
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(body);
                string status = doc
                    .RootElement.GetProperty("data")
                    .GetProperty("status")
                    .GetString()!;
                Console.WriteLine($"Status: {status}");

                if (status == "finished")
                    return;
                if (status is "failed" or "canceled")
                    throw new InvalidOperationException(
                        $"Crowdin build failed with status: {status}"
                    );

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }

        private async Task<string> GetDownloadUrl(string buildId)
        {
            using HttpResponseMessage response = await httpClient.GetAsync(
                $"projects/{projectId}/bundles/1/exports/{buildId}/download"
            );
            response.EnsureSuccessStatusCode();
            string body = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(body);
            return doc.RootElement.GetProperty("data").GetProperty("url").GetString()
                ?? throw new InvalidOperationException("Crowdin download response missing url.");
        }

        private static async Task DownloadFile(string url)
        {
            using var client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            Directory.CreateDirectory(Program.LocalDir);

            string zipPath = Path.Combine(Program.LocalDir, $"download.zip");
            string lastPath = Path.Combine(Program.LocalDir, "last.txt");

            using FileStream fs = new(zipPath, FileMode.Create, FileAccess.Write);
            await response.Content.CopyToAsync(fs);

            await File.WriteAllTextAsync(lastPath, Program.now.ToString());
        }
    }
}
