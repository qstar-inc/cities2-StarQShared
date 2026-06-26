using System.IO;

namespace StarQ.Shared.Extensions
{
    public partial class FileHelper
    {
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
    }
}
