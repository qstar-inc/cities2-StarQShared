using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace StarQ.Shared.Extensions
{
    public static class FileHelper
    {
        public class FileEntry
        {
            [JsonProperty("path")]
            private string Path { get; set; } = string.Empty;

            [JsonProperty("size")]
            private long Size { get; set; }

            public FileEntry(string path, long size)
            {
                Path = path;
                Size = size;
            }
        }

        public static string AddLongPathPrefix(string path) =>
            path.StartsWith(@"\\?\") ? path : @"\\?\" + Path.GetFullPath(path);

        public static bool FilesAreEqual(string file1, string file2)
        {
            using FileStream fs1 = File.OpenRead(file1);
            using FileStream fs2 = File.OpenRead(file2);

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

        public static void CopyDirectory(string source, string destination)
        {
            Directory.CreateDirectory(destination);

            foreach (string file in Directory.GetFiles(source))
            {
                string targetFile = Path.Combine(destination, Path.GetFileName(file));
                File.Copy(file, targetFile, true);
            }

            foreach (string directory in Directory.GetDirectories(source))
            {
                string targetDirectory = Path.Combine(destination, Path.GetFileName(directory));
                CopyDirectory(directory, targetDirectory);
            }
        }

        public static void DeleteDirectorySafe(string path)
        {
            foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }

            foreach (
                string directory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
            )
            {
                File.SetAttributes(directory, FileAttributes.Normal);
            }

            File.SetAttributes(path, FileAttributes.Normal);
            Directory.Delete(path, true);
        }

        public static List<FileEntry> ListFiles(string root, bool ignoreMeta)
        {
            List<FileEntry> result = new();
            foreach (
                string file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
            )
            {
                string relative = Path.GetRelativePath(root, file).Replace('\\', '/');

                if (ignoreMeta && IsMetaPath(relative))
                    continue;

                long fileSize = new FileInfo(file).Length;

                if (!IsZipFile(file))
                {
                    result.Add(new FileEntry(relative, fileSize));
                    continue;
                }

                List<FileEntry> entries = new();
                try
                {
                    using ZipArchive? archive = ZipFile.OpenRead(file);
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith('/'))
                            continue;

                        entries.Add(new FileEntry($"{relative}@{entry.FullName}", entry.Length));
                    }
                    result.AddRange(entries);
                }
                catch (Exception ex)
                    when (ex is InvalidDataException or IOException or NotSupportedException)
                {
                    result.Add(new FileEntry(relative, fileSize));
                }
            }
            return result;
        }

        public static bool IsMetaPath(string path)
        {
            foreach (string segment in path.Split('/', '\\'))
            {
                if (
                    segment.EndsWith(".metadata", StringComparison.OrdinalIgnoreCase)
                    || segment.EndsWith(".cpatch", StringComparison.OrdinalIgnoreCase)
                )
                    return true;
            }
            return false;
        }

        public static bool IsZipFile(string path)
        {
            try
            {
                using FileStream? fs = File.OpenRead(path);
                Span<byte> sig = stackalloc byte[4];
                if (fs.Read(sig) < 4)
                    return false;

                return sig[0] == 'P'
                    && sig[1] == 'K'
                    && (
                        (sig[2] == 3 && sig[3] == 4)
                        || (sig[2] == 5 && sig[3] == 6)
                        || (sig[2] == 7 && sig[3] == 8)
                    );
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}
