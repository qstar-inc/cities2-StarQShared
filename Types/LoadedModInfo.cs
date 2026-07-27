using PDX.SDK.Contracts.Service.Mods.Enums;

namespace StarQ.Shared.Types
{
    public class LoadedModInfo
    {
        public string? Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Author { get; set; }
        public string? Version { get; set; }
        public string? LatestVersion { get; set; }
        public string? UserModVersion { get; set; }
        public ulong Size { get; set; }
        public bool Active { get; set; }
        public ModAccessControlLevelState? AccessControl { get; set; }
    }
}
