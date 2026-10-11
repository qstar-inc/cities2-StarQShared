using static Colossal.IO.AssetDatabase.ImageAsset;

namespace StarQ.Shared.Extensions
{
    public static class TextureHelper
    {
        public static (bool, FileFormat) TryExtractImageFromTextureAsset(
            Colossal.IO.AssetDatabase.TextureAsset textureAsset,
            out byte[] data
        )
        {
            data = null;

            FileFormat ff = FileFormat.PNG;

            if (textureAsset == null)
                return (false, ff);

            if (textureAsset.ToImageData(FileFormat.PNG, out uint _, out uint _, out byte[] data1))
            {
                data = data1;
                ff = FileFormat.PNG;
            }
            else if (
                textureAsset.ToImageData(FileFormat.JPG, out uint _, out uint _, out byte[] data2)
            )
            {
                data = data2;
                ff = FileFormat.JPG;
            }
            else if (
                textureAsset.ToImageData(FileFormat.TIFF, out uint _, out uint _, out byte[] data3)
            )
            {
                data = data3;
                ff = FileFormat.TIFF;
            }
            else
                return (false, ff);

            return (true, ff);
        }
    }
}
