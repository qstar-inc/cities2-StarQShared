using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StarQ.Shared.Extensions
{
    public static class ChecksumHelper
    {
        public enum Sha256OutputType
        {
            Base64,
            Hex64,
        }

        public static string Sha256FromBytes(byte[] hashBytes)
        {
            return string.Concat(hashBytes.Select(b => b.ToString("x2")));
        }

        public static string? Sha256FromFile(
            string filePath,
            Sha256OutputType outputType = Sha256OutputType.Base64
        )
        {
            try
            {
                filePath = FileHelper.AddLongPathPrefix(filePath);
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

        public static string CreateMD5(string checksumInput)
        {
            using MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(checksumInput));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
