using Colossal.IO.AssetDatabase;
using Colossal.UI;

namespace StarQ.Shared.Extensions
{
    public class UIHostHelper
    {
        public static string Id;
        public static string uiHostName;

        public static void Init(string _id, string _uiHostName)
        {
            Id = _id;
            uiHostName = _uiHostName;
        }

        public static string IconHostName => uiHostName ?? null;

        public static string Icon(string iconName, string extension = "svg")
        {
            if (IconHostName == null)
                return MGI("Placeholder");
            return $"coui://{IconHostName}/{iconName}.{extension}";
        }

        public static string MGI(string iconName) => $"Media/Game/Icons/{iconName}.svg";

        public static string DLC(string iconName) => $"Media/DLC/{iconName}.svg";

        public static void LoadUIHost(ExecutableAsset asset)
        {
            //LogHelper.SendLog(asset.subPath, LogLevel.DEV);
            var uisystem = UIManager.defaultUISystem;
            var x = AssetDatabase.global.GetAssets<UIHostAsset>().GetEnumerator();
            while (x.MoveNext())
            {
                var uihostAsset = x.Current;
                //LogHelper.SendLog(uihostAsset.path, LogLevel.DEV);
                if (uihostAsset.path.Contains("Cities2_Data/Content"))
                    continue;

                if (uihostAsset.path.Contains(asset.subPath))
                    if (uihostAsset.scheme == "assetdb")
                    {
                        uisystem.AddDatabaseHostLocation(
                            uihostAsset.hostname,
                            uihostAsset.uiUri,
                            uihostAsset.priority
                        );
                        //LogHelper.SendLog("AddDatabaseHostLocation", LogLevel.DEV);
                    }
                    else
                    {
                        uisystem.AddHostLocation(
                            uihostAsset.hostname,
                            uihostAsset.uiPath,
                            true,
                            uihostAsset.priority
                        );
                        //LogHelper.SendLog("AddHostLocation", LogLevel.DEV);
                    }
            }
        }
    }
}
