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

        public static string Icon(string iconName)
        {
            if (IconHostName == null)
                return MGI("Placeholder");
            return $"coui://{IconHostName}/{iconName}";
        }

        public static string MGI(string iconName) => $"Media/Game/Icons/{iconName}.svg";
    }
}
