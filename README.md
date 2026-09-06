## Environment variables:
- CS2_GAMEVERSION : The game version (update after every public patch).
- CS2_FORUMLINK : Fixed link to PDX Forum Megathread.
Modding Toolchain variables are also required.

## .csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <AssemblyName>XXX</AssemblyName>
    <AssemblyTitle>XXXX</AssemblyTitle>
    <Version>X.X.X</Version>
    <FirstInit>true</FirstInit>
    <!--The file where mod information which is required for publishing mod on PDX mods are stored-->
    <PublishConfigurationPath>Properties\PublishConfiguration.xml</PublishConfigurationPath>
  </PropertyGroup>
  <!--Imports must be after PropertyGroup block-->
  <Import Project="$([System.Environment]::GetEnvironmentVariable('CSII_TOOLPATH', 'EnvironmentVariableTarget.User'))\Mod.props" />
  <Import Project="$([System.Environment]::GetEnvironmentVariable('CSII_TOOLPATH', 'EnvironmentVariableTarget.User'))\Mod.targets" />
  <Import Project="$(CS2_STARQ_SHARED_PATH)\StarQ.Shared.projitems" Label="Shared" />
  <Import Project="$(CS2_STARQ_SHARED_PATH)\StarQ.Shared.props" />
  <Import Project="$(CS2_STARQ_SHARED_PATH)\StarQ.Shared.targets" />
</Project>
```

* For old lang systems,
```
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
    [GenerateModInfo]
    public partial class Mod : IMod
    {
        public void OnLoad(UpdateSystem updateSystem)
        {
            LogHelper.Init(Id, log);
            LocaleHelper.Init(Id, Name, GetReplacements);
            
            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            AssetDatabase.global.LoadSettings(Id, m_Setting, new Setting(this));
        }

        public void OnDispose()
        {
            LocaleHelper.Dispose();
            m_Setting?.UnregisterInOptionsUI();
            m_Setting = null;
        }

        public static Dictionary<string, string> GetReplacements()
        {
            return new() { };
        }
    }
```
* If there is UIHost,
```
        public static string uiHostName = "starq-xxxxxxx";
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
    [GenerateSettingCommonAttribute]
    public partial class Setting : ModSetting
    {
        public override void SetDefaults() { }
    }
```

## tsconfig.json
```
    "paths": {
      "mod.json": ["../mod.json"]
    },
    "preserveSymlinks": true
```

## webpack.json
```
        test: /\.s?css$/,
        include: [
          path.join(__dirname, "src"),
          path.resolve(__dirname, "../StarQ.Shared/UI/styles"),
        ],
```
```
  resolve: {
    symlinks: false,
    extensions: [".tsx", ".ts", ".js"],
```

## create symlinks commands
```
Start-Process powershell -Verb RunAs -ArgumentList "-NoProfile -Command `"New-Item -ItemType SymbolicLink -Path '$PWD\shared' -Target 'D:\_Q\Codes\qstar-inc\ModsPersonal\StarQ.Shared\UI\shared'; New-Item -ItemType SymbolicLink -Path '$PWD\styles' -Target 'D:\_Q\Codes\qstar-inc\ModsPersonal\StarQ.Shared\UI\styles'`""
```