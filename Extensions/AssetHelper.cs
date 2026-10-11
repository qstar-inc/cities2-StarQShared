using Colossal.IO.AssetDatabase;

namespace StarQ.Shared.Extensions
{
    public static class AssetHelper
    {
        public static bool TryGetAsset(string url, out AssetData? assetData)
        {
            assetData = null;
            if (!AssetDatabase.global.TryGetAsset(url, out assetData))
                return false;

            return true;
        }

        public static bool TryGetAssetGUID(string url, out string guid)
        {
            guid = string.Empty;
            if (!TryGetAsset(url, out AssetData? asset) || asset == null)
                return false;

            guid = asset.id.guid.ToString();
            return true;
        }
    }
}
