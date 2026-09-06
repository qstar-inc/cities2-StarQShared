using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using LocaleManagement.Types;

namespace LocaleManagement
{
    internal static partial class Program
    {
        public static Options options = new();
        public static string LocalDir = string.Empty;
        public static long now = 0;

        public static async Task<int> Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("This application must be run with arguments.");
                Console.Error.WriteLine("Usage: LocaleManagement.exe -mod <modName>");
                return 1;
            }

            now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

#if DEBUG
            LocalDir = Path.Combine(
                GetEnvValue("CS2_STARQ_SHARED_PATH"),
                "Projects",
                "StarQ.Shared.LocaleManagement",
                "cache"
            );
#else
            LocalDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "StarQ",
                "Cities Skylines II Crowdin LocaleManagement"
            );
#endif

            string downloadFile = Path.Combine(LocalDir, "download.zip");

            try
            {
                options = GetOptions(args);
                if (options.Force || now - options.CacheTime >= 24 * 60 * 60)
                {
                    CleanupCacheFolder();
                    await DownloadFromCrowdin();
                }

                if (!File.Exists(downloadFile))
                {
                    throw new Exception(
                        "Downloaded file does not exist. This should never happen."
                    );
                }

                ExtractAndOrganizeZip();
                CreateLoc();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1;
            }

            return 0;
        }

        private static async Task DownloadFromCrowdin()
        {
            string token = GetEnvValue("CS2_CROWDIN_API_TOKEN");

            var client = new CrowdinClient(token!, options.ProjectId!);
            Console.WriteLine($"Starting download");

            if (!await client.BuildAndDownload())
            {
                Console.WriteLine("Build download unavailable; continuing without it.");
            }
        }

        private static void ExtractAndOrganizeZip()
        {
            string outputDir = Path.Combine(LocalDir, "output");
            if (Directory.Exists(outputDir))
                Directory.Delete(outputDir, true);

            string zipPath = Path.Combine(LocalDir, "download.zip");
            using ZipArchive archive = ZipFile.OpenRead(zipPath);

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (string.IsNullOrEmpty(entry.Name))
                    continue;

                string[] parts = entry.FullName.Trim('/').Split('/');

                string[] relativeParts = parts.Length > 1 ? [.. parts.Skip(1)] : parts;

                for (int i = 1; i < relativeParts.Length - 1; i++)
                {
                    relativeParts[i] = relativeParts[i].Replace(" ", "");
                }

                string relativePath = Path.Combine(relativeParts);

                string targetPath = Path.Combine(outputDir, relativePath);
                string targetDir = Path.GetDirectoryName(targetPath) ?? string.Empty;

                if (!string.IsNullOrEmpty(targetDir))
                    Directory.CreateDirectory(targetDir);

                entry.ExtractToFile(targetPath, overwrite: true);
            }
            Console.WriteLine($"Extraction complete");
        }

        private static void CreateLoc()
        {
            Console.WriteLine($"Creating loc files");
            Dictionary<string, Dictionary<string, string>> locales = OrganizeLocales();

            WriteLocFiles(locales);
        }

        private static Dictionary<string, string> ReadEntries(string file)
        {
            return JsonSerializer.Deserialize(
                    File.ReadAllText(file),
                    AppJsonContext.Default.DictionaryStringString
                ) ?? throw new InvalidDataException($"Failed to deserialize language file: {file}");
        }

        private static string ProcessKey(string key)
        {
            if (key.StartsWith("Option", StringComparison.Ordinal))
            {
                if (FormatOptionsText(key, out string locale))
                    return locale;
            }

            return $"{options.ModName}.{key}";
        }

        private static Dictionary<string, Dictionary<string, string>> OrganizeLocales()
        {
            Dictionary<string, Dictionary<string, string>> groupedData = [];

            string modJsonDir = Path.Combine(LocalDir, "output", options.ModName);
            string commonDir = Path.Combine(LocalDir, "output", "_Common");

            if (Directory.Exists(modJsonDir))
            {
                foreach (string path in Directory.GetFiles(modJsonDir))
                {
                    string langCode = Path.GetFileNameWithoutExtension(path);

                    if (!LangMap.languageMap.ContainsKey(langCode))
                        continue;

                    if (!groupedData.TryGetValue(langCode, out Dictionary<string, string>? locale))
                    {
                        locale = [];
                        groupedData[langCode] = locale;
                    }

                    Dictionary<string, string> jsonData = ReadEntries(path);

                    foreach (KeyValuePair<string, string> entry in jsonData)
                    {
                        string key = ProcessKey(entry.Key);

                        locale[key] = entry.Value;
                    }
                }
            }

            if (Directory.Exists(commonDir))
            {
                foreach (string path in Directory.GetFiles(commonDir))
                {
                    string langCode = Path.GetFileNameWithoutExtension(path);

                    if (!LangMap.languageMap.ContainsKey(langCode))
                        continue;

                    if (!groupedData.TryGetValue(langCode, out Dictionary<string, string>? locale))
                    {
                        locale = [];
                        groupedData[langCode] = locale;
                    }

                    Dictionary<string, string> jsonData = ReadEntries(path);

                    foreach (KeyValuePair<string, string> entry in jsonData)
                    {
                        string key = ProcessKey(entry.Key);

                        if (!locale.ContainsKey(key))
                            locale[key] = entry.Value;
                    }
                }
            }

            ValidateAndFillLocales(groupedData);

            return groupedData;
        }

        private static void WriteLocFiles(Dictionary<string, Dictionary<string, string>> locales)
        {
            string outputDir = Path.Combine(LocalDir, "loc");

            if (!string.IsNullOrEmpty(options.Output))
                outputDir = options.Output;

            Directory.CreateDirectory(outputDir);

            int i = 0;
            foreach (KeyValuePair<string, Dictionary<string, string>> locale in locales)
            {
                i++;
                WriteLocFile(locale.Key, locale.Value, outputDir);
            }

            Console.WriteLine($"Done processing {i} locale files");
        }

        private static void WriteLocFile(
            string lang,
            Dictionary<string, string> entries,
            string outputDir
        )
        {
            if (!LangMap.languageMap.TryGetValue(lang, out LangData? langData))
                throw new InvalidDataException($"'{lang}' is not a valid language.");

            string output = Path.Combine(outputDir, $"{options.ModName}_{lang}.loc");

            using FileStream stream = File.Create(output);
            using BinaryWriter binaryWriter = new(stream, new UTF8Encoding(false));

            binaryWriter.Write((ushort)1);
            binaryWriter.Write(langData.Name);
            binaryWriter.Write(lang);
            binaryWriter.Write(langData.LocalizedName);

            binaryWriter.Write(entries.Count);

            foreach (KeyValuePair<string, string> entry in entries)
            {
                binaryWriter.Write(entry.Key);
                binaryWriter.Write(entry.Value);
            }

            binaryWriter.Write(0);
        }

        private static void ValidateAndFillLocales(
            Dictionary<string, Dictionary<string, string>> groupedData
        )
        {
            const string EnglishLang = "en-US";

            if (!groupedData.TryGetValue(EnglishLang, out Dictionary<string, string>? english))
            {
                throw new InvalidDataException($"English locale '{EnglishLang}' not found.");
            }

            foreach (KeyValuePair<string, Dictionary<string, string>> pair in groupedData)
            {
                string langCode = pair.Key;
                Dictionary<string, string> locale = pair.Value;

                if (langCode == EnglishLang)
                    continue;

                string[] extra = locale.Keys.Except(english.Keys).OrderBy(x => x).ToArray();

                if (extra.Length > 0)
                {
                    throw new InvalidDataException(
                        $"{langCode} contains keys not present in {EnglishLang}:\n"
                            + string.Join('\n', extra)
                    );
                }

                int missingCount = 0;

                foreach (KeyValuePair<string, string> entry in english)
                {
                    if (locale.ContainsKey(entry.Key))
                        continue;

                    locale[entry.Key] = entry.Value;
                    missingCount++;
                }

                if (missingCount > 0)
                {
                    Console.WriteLine(
                        $"{langCode}: copied {missingCount} missing entries from {EnglishLang}"
                    );
                }
            }
        }

        public static bool FormatOptionsText(string text, out string value)
        {
            string modName = options.ModName;
            value = string.Empty;
            if (text.StartsWith("Option."))
            {
                Match match = OptionRegex1().Match(text);

                if (match.Success)
                {
                    string item = match.Groups[1].Value;
                    string type = match.Groups[2].Value;

                    switch (type)
                    {
                        case "Label":
                        {
                            value = $"Options.OPTION[{modName}.{modName}.Mod.Setting.{item}]";
                            return true;
                        }
                        case "Desc":
                        case "Description":
                        {
                            value =
                                $"Options.OPTION_DESCRIPTION[{modName}.{modName}.Mod.Setting.{item}]";
                            return true;
                        }
                        default:
                            return false;
                    }
                }
            }
            else if (text.StartsWith("OptionsEnum."))
            {
                Match match = OptionRegex2().Match(text);

                if (match.Success)
                {
                    string enumGroup = match.Groups[1].Value;
                    string enumValue = match.Groups[2].Value;

                    value =
                        $"Options.{modName}.{modName}.Mod.{enumGroup.ToUpperInvariant()}[{enumValue}]";
                    return true;
                }
            }
            else
            {
                var match = OptionRegex3().Match(text);

                if (match.Success)
                {
                    string prefix = match.Groups[1].Value;
                    string name = match.Groups[2].Value;

                    if (prefix is "Group" or "Tab")
                    {
                        value =
                            $"Options.{prefix.ToUpperInvariant()}[{modName}.{modName}.Mod.{name}]";
                        return true;
                    }
                }
            }

            return false;
        }

        [GeneratedRegex(@"^Option\.(.+)\.(.+)$")]
        private static partial Regex OptionRegex1();

        [GeneratedRegex(@"^OptionsEnum\.(.+)\.(.+)$")]
        private static partial Regex OptionRegex2();

        [GeneratedRegex(@"^Option(.+)\.(.+)$")]
        private static partial Regex OptionRegex3();
    }
}
