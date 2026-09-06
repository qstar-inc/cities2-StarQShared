using LocaleManagement.Types;

namespace LocaleManagement
{
    internal static partial class Program
    {
        internal static Options GetOptions(string[] args)
        {
            var options = new Options { ProjectId = GetEnvValue("CS2_CROWDIN_PROJECT_ID") };

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (!arg.StartsWith("-"))
                    continue;

                string santiziedArg = arg.TrimStart('-');

                switch (santiziedArg)
                {
                    case "projectId":
                        options.ProjectId = GetArgValue("projectId", args, i + 1);
                        break;
                    case "bundleId":
                        options.BundleId = GetArgValue("bundleId", args, i + 1);
                        break;
                    case "mod":
                        options.ModName = GetArgValue("mod", args, i + 1);
                        break;
                    case "force":
                        options.Force = true;
                        break;
                    case "output":
                    {
                        string argVal = GetArgValue("output", args, i + 1);
                        if (IsValidPath(argVal, out string validatedPath))
                            options.Output = validatedPath;
                        break;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(options.ModName))
                throw new ArgumentException("Missing required argument: -mod <modName>");

            if (Directory.Exists(LocalDir))
            {
                string filePath = Path.Combine(LocalDir, "last.txt");

                if (Path.Exists(filePath))
                {
                    string fileText = File.ReadAllText(filePath);
                    if (long.TryParse(fileText, out long cacheTime))
                        options.CacheTime = cacheTime;
                }
            }

            return options;
        }

        internal static void CleanupCacheFolder()
        {
            if (Directory.Exists(LocalDir))
                Directory.Delete(LocalDir, true);

            Directory.CreateDirectory(LocalDir);
        }

        internal static string GetArgValue(string argName, string[] args, int j)
        {
            if (args.Length < j + 1)
                throw new ArgumentException($"{argName} argument is missing.");

            return args[j];
        }

        internal static string GetEnvValue(string envName)
        {
            string envVal = Environment.GetEnvironmentVariable(envName) ?? string.Empty;

            if (string.IsNullOrEmpty(envVal))
                throw new ArgumentException($"No Environment Variable set for {envName}");

            return envVal;
        }

        internal static bool IsValidPath(string path, out string validPath)
        {
            validPath = string.Empty;

            if (string.IsNullOrWhiteSpace(path))
                return false;

            try
            {
                path = path.TrimStart('/', '\\');

                validPath = Path.GetFullPath(path);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (PathTooLongException)
            {
                return false;
            }
        }
    }
}
