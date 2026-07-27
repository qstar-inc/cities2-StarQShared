using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;

namespace StarQ.Shared.Extensions
{
    public partial class FileHelper
    {
        public static string AddLongPathPrefix(string path) =>
            path.StartsWith(@"\\?\") ? path : @"\\?\" + Path.GetFullPath(path);

        public static bool FilesAreEqual(string file1, string file2)
        {
            using var fs1 = File.OpenRead(file1);
            using var fs2 = File.OpenRead(file2);

            if (fs1.Length != fs2.Length)
                return false;

            int b1;
            int b2;

            do
            {
                b1 = fs1.ReadByte();
                b2 = fs2.ReadByte();
            } while (b1 == b2 && b1 != -1);

            return b1 == b2;
        }

        public enum Sha256OutputType
        {
            Base64,
            Hex64,
        }

        public static string? Sha256FromFile(
            string filePath,
            Sha256OutputType outputType = Sha256OutputType.Base64
        )
        {
            try
            {
                filePath = AddLongPathPrefix(filePath);
                using SHA256 sha256 = SHA256.Create();

                using FileStream stream = new(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    1 << 20,
                    FileOptions.SequentialScan
                );

                byte[] hashBytes = sha256.ComputeHash(stream);

                switch (outputType)
                {
                    case Sha256OutputType.Base64:
                        return Convert
                            .ToBase64String(hashBytes)
                            .Replace("/", "_")
                            .Replace("+", "-");
                    case Sha256OutputType.Hex64:
                        return Sha256FromBytes(hashBytes);
                }
            }
            catch (UnauthorizedAccessException)
            {
                LogHelper.SendLog($"Access to file '{filePath}' is denied.");
            }
            catch (IOException ex)
            {
                LogHelper.SendLog($"Error computing SHA256 for file '{filePath}': {ex.Message}");
            }
            catch (Exception ex)
            {
                LogHelper.SendLog(ex);
            }
            return null;
        }

        public static string Sha256FromBytes(byte[] hashBytes)
        {
            return string.Concat(hashBytes.Select(b => b.ToString("x2")));
        }

        public static string GetRelativePath(string basePath, string fullPath)
        {
            try
            {
                Uri baseUri = new(
                    basePath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar
                );
                Uri fullUri = new(fullPath);
                return Uri.UnescapeDataString(
                    baseUri
                        .MakeRelativeUri(fullUri)
                        .ToString()
                        .Replace('/', Path.DirectorySeparatorChar)
                );
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to compute relative path for '{fullPath}': {ex.Message}",
                    ex
                );
            }
        }
    }
}
