namespace LocaleManagement.Types
{
    internal class Options
    {
        public string ProjectId { get; set; } = string.Empty;
        public string BundleId { get; set; } = "1";
        public string ModName { get; set; } = string.Empty;
        public bool Force { get; set; } = false;
        public long CacheTime { get; set; } = 0;
        public string Output { get; set; } = string.Empty;
    }
}
