namespace LocaleManagement.Types
{
    internal class LangData
    {
        public string Name { get; set; } = string.Empty;
        public string LocalizedName { get; set; } = string.Empty;
    }

    internal static class LangMap
    {
        internal static Dictionary<string, LangData> languageMap = new()
        {
            ["en-US"] = new LangData { Name = "English", LocalizedName = "English" },
            ["de-DE"] = new LangData { Name = "German", LocalizedName = "Deutsch" },
            ["es-ES"] = new LangData { Name = "Spanish", LocalizedName = "Español" },
            ["fr-FR"] = new LangData { Name = "French", LocalizedName = "Français" },
            ["it-IT"] = new LangData { Name = "Italian", LocalizedName = "Italian" },
            ["ja-JP"] = new LangData { Name = "Japanese", LocalizedName = "日本語" },
            ["ko-KR"] = new LangData { Name = "Korean", LocalizedName = "한국어" },
            ["pl-PL"] = new LangData { Name = "Polish", LocalizedName = "Polski" },
            ["pt-BR"] = new LangData
            {
                Name = "Portuguese",
                LocalizedName = "Português brasileiro",
            },
            ["ru-RU"] = new LangData { Name = "Russian", LocalizedName = "Русский" },
            ["zh-HANS"] = new LangData { Name = "ChineseSimplified", LocalizedName = "汉语" },
            ["zh-HANT"] = new LangData { Name = "ChineseTraditional", LocalizedName = "繁體中文" },
            ["hi"] = new LangData { Name = "Hindi", LocalizedName = "हिन्दी" },
            ["bn"] = new LangData { Name = "Bangla", LocalizedName = "বাংলা" },
        };
    }
}
