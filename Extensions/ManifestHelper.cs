using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace StarQ.Shared.Extensions
{
    public class ManifestHelper
    {
        private static readonly Regex csvRegex = new(
            @"(?:^|,)(?:(?:""(?<value>[^""]*)"")|(?<value>[^,""]*))",
            RegexOptions.Compiled
        );
        public static Dictionary<string, Dictionary<string, string>> ManifestData = new();

        public static Dictionary<string, string> ReadManifestFile(string? manifestPath)
        {
            if (manifestPath is null)
            {
                LogHelper.SendLog($"{manifestPath} is null");
                return new();
            }

            if (string.IsNullOrEmpty(manifestPath))
            {
                LogHelper.SendLog($"{manifestPath} is empty");
                return new();
            }

            if (ManifestData.TryGetValue(manifestPath, out var cached))
                return cached;

            var manifestData = new Dictionary<string, string>(StringComparer.Ordinal);

            foreach (var line in File.ReadLines(manifestPath))
            {
                var parts = csvRegex
                    .Matches(line)
                    .Cast<Match>()
                    .Select(m => m.Groups["value"].Value)
                    .ToList();

                if (parts.Count >= 4)
                {
                    string relativePath = parts[0].Trim('"').Replace("/", "\\");
                    string size = parts[1];
                    string hash = parts[2];
                    manifestData[relativePath] = $"{size};;{hash}";
                }
            }

            ManifestData[manifestPath] = manifestData;
            return manifestData;
        }

        public static string? FindManifestFile(string subfolder)
        {
            string cpatchFolder = Path.Combine(subfolder, ".cpatch");
            if (!Directory.Exists(cpatchFolder))
                return null;
            try
            {
                string folderName = Path.GetFileName(Path.GetDirectoryName(cpatchFolder));
                if (folderName == null || !folderName.Contains("_"))
                    return null;

                string version = folderName.Split('_')[1];

                var randomFolders = Directory.GetDirectories(
                    cpatchFolder,
                    "*",
                    SearchOption.TopDirectoryOnly
                );

                foreach (var folder in randomFolders)
                {
                    string manifestPath = Path.Combine(folder, version, "complete", "manifest");
                    if (File.Exists(manifestPath))
                        return manifestPath;
                }
            }
            catch (Exception ex)
            {
                LogHelper.SendLog($"Error searching for manifest in '.cpatch': {ex.Message}");
            }

            return null;
        }
    }
}
