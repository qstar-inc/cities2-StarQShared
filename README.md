## .csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <Configurations>Debug;Release</Configurations>
    <AssemblyName>XXX</AssemblyName>
    <AssemblyTitle>XXXX</AssemblyTitle>
    <Version>X.X.X</Version>
    <Authors>StarQ</Authors>
```
```
  <!--Imports must be after PropertyGroup block-->
  <Import Project="$([System.Environment]::GetEnvironmentVariable('CSII_TOOLPATH', 'EnvironmentVariableTarget.User'))\Mod.props" />
  <Import Project="$([System.Environment]::GetEnvironmentVariable('CSII_TOOLPATH', 'EnvironmentVariableTarget.User'))\Mod.targets" />
  <Import Project="..\..\ModsPersonal\StarQ.Shared\StarQ.Shared.projitems" Label="Shared" />
```
```
  <ItemGroup>
    <None Include="$(ModPropsFile)" Link="Properties\Mod.props" />
    <None Include="$(ModTargetsFile)" Link="Properties\Mod.targets" />
  </ItemGroup>
  <Target Name="GetAdditionalFiles" AfterTargets="DeployWIP">
    <ItemGroup>
      <AdditionalFilesToDeploy Include="Resources\**\*.*" />
    </ItemGroup>
    <Copy
      SourceFiles="@(AdditionalFilesToDeploy)"
      DestinationFiles="@(AdditionalFilesToDeploy-&gt;'$(DeployDir)\%(RecursiveDir)%(Filename)%(Extension)')"
    />
  </Target>
  <ItemGroup>
    <EmbeddedResource Include="Locale.json" />
    <EmbeddedResource Include="Locale\*.json" />
    <None Remove="Locale.json" />
  </ItemGroup>
  <ItemGroup>
    <Folder Include="Locale\" />
  </ItemGroup>
```

## Mod.cs
```
    public class Mod : IMod
    {
        public static string Id = nameof(XXX);
        public static string Name = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyTitleAttribute>()
            .Title;
        public static string Version = Assembly
            .GetExecutingAssembly()
            .GetName()
            .Version.ToString(3);

        public static ILog log = LogManager.GetLogger($"{Id}").SetShowsErrorsInUI(false);
        public static Setting m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            LogHelper.Init(Id, log);
            LocaleHelper.Init(Id, Name, GetReplacements);
```
```
        public static Dictionary<string, string> GetReplacements()
        {
            return new() { { "X", "Y" } };
        }
```
* If there is UIHost,
```
        public static string uiHostName = "starq-asset-ui-manager";
```
```
            LocaleHelper.Init(Id, Name, GetReplacements);
            UIHostHelper.Init(Id, uiHostName);
```
```
            locMan.onActiveDictionaryChanged += LocaleHelper.OnActiveDictionaryChanged;

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                UIManager.defaultUISystem.AddHostLocation(
                    uiHostName,
                    Path.Combine(Path.GetDirectoryName(asset.path), "Icons"),
                    false
                );
```

## Setting.cs
```
    [FileLocation("ModsSettings\\StarQ\\" + nameof(XXX))]
    [SettingsUITabOrder(GeneralTab, AboutTab, LogTab)]
    public class Setting : ModSetting
    {
        public Setting(IMod mod)
            : base(mod) => SetDefaults();

        public const string GeneralTab = "GeneralTab";
        public const string GeneralGroup = "GeneralGroup";

        public const string AboutTab = "AboutTab";
        public const string InfoGroup = "InfoGroup";

        public const string LogTab = "LogTab";
```
```
        [SettingsUISection(AboutTab, InfoGroup)]
        public string NameText => Mod.Name;

        [SettingsUISection(AboutTab, InfoGroup)]
        public string VersionText => VariableHelper.AddDevSuffix(Mod.Version);

        [SettingsUISection(AboutTab, InfoGroup)]
        public string AuthorText => VariableHelper.StarQ;

        [SettingsUIButton]
        [SettingsUIButtonGroup("Social")]
        [SettingsUISection(AboutTab, InfoGroup)]
        public bool BMaCLink
        {
            set => VariableHelper.OpenBMAC();
        }

        [SettingsUIButton]
        [SettingsUIButtonGroup("Social")]
        [SettingsUISection(AboutTab, InfoGroup)]
        public bool Discord
        {
            set => VariableHelper.OpenDiscord(XXXX);
        }

        [SettingsUIMultilineText]
        [SettingsUIDisplayName(typeof(LogHelper), nameof(LogHelper.LogText))]
        [SettingsUISection(LogTab, "")]
        public string LogText => string.Empty;

        [Exclude]
        [SettingsUIHidden]
        public bool IsLogMissing
        {
            get => VariableHelper.CheckLog(Mod.Id);
        }

        [SettingsUIButton]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsLogMissing))]
        [SettingsUISection(LogTab, "")]
        public bool OpenLog
        {
            set => VariableHelper.OpenLog(Mod.Id);
        }
```
