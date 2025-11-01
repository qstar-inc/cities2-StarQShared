using System.Collections.Generic;
using Game.SceneFlow;

namespace StarQ.Shared.Extensions
{
    public partial class LocaleHelper
    {
        private static readonly Dictionary<string, Dictionary<string, string>> CommonLocales = new()
        {
            ["en-US"] = new()
            {
                ["LogTab"] = "Log",
                ["NameText"] = "Mod Name",
                ["NameText_Desc"] = "The name of the mod.",
                ["VersionText"] = "Mod Version",
                ["VersionText_Desc"] = "The current version of the mod.",
                ["AuthorText"] = "Author",
                ["AuthorText_Desc"] = "The author of the mod.",
                ["BMaCLink_Desc"] = "Support the author.",
                ["Discord_Desc"] =
                    "Feedback/Suggestions for the mod in the Cities: Skylines Modding Server.",
                ["OpenLog"] = "Open Log File",
                ["OpenLog_Desc"] = "Open the log file in your default <.log> file viewer/editor.",
                ["RestoreToVanilla"] = "Restore Vanilla Values",
                ["RestoreToVanilla_Desc"] = "Restore all options to vanilla values.",
                ["ReloadToApply"] = "Reload save to apply changes.",
                ["ReloadToApply_Desc"] = "Reload save to apply changes.",
                ["NoLog"] = "No log entries yet.",
                ["PerXPop"] = "per {X} population",
                ["NotInGame"] = "Not in game",
            },
            ["de-DE"] = new()
            {
                ["LogTab"] = "Log",
                ["NameText"] = "Mod Name",
                ["NameText_Desc"] = "The name of the mod.",
                ["VersionText"] = "Mod Version",
                ["VersionText_Desc"] = "The current version of the mod.",
                ["AuthorText"] = "Author",
                ["AuthorText_Desc"] = "The author of the mod.",
                ["BMaCLink_Desc"] = "Support the author.",
                ["Discord_Desc"] =
                    "Feedback/Suggestions for the mod in the Cities: Skylines Modding Server.",
                ["OpenLog"] = "Open Log File",
                ["OpenLog_Desc"] = "Open the log file in your default <.log> file viewer/editor.",
                ["RestoreToVanilla"] = "Restore Vanilla Values",
                ["RestoreToVanilla_Desc"] = "Restore all options to vanilla values.",
                ["ReloadToApply"] = "Reload save to apply changes.",
                ["ReloadToApply_Desc"] = "Reload save to apply changes.",
                ["NoLog"] = "No log entries yet.",
                ["PerXPop"] = "per {X} population",
                ["NotInGame"] = "Not in game",
            },
            ["es-ES"] = new()
            {
                ["LogTab"] = "Registro",
                ["NameText"] = "Nombre del Mod",
                ["NameText_Desc"] = "El nombre del mod.",
                ["VersionText"] = "Versión del Mod",
                ["VersionText_Desc"] = "La versión actual del mod.",
                ["AuthorText"] = "Autor",
                ["AuthorText_Desc"] = "El autor del mod.",
                ["BMaCLink_Desc"] = "Apoya al autor del mod.",
                ["Discord_Desc"] =
                    "Comentarios/Sugerencias para el mod en el servidor Cities: Skylines Modding.",
                ["OpenLog"] = "Abrir archivo de registro",
                ["OpenLog_Desc"] =
                    "Abre el archivo de registro <.log> con el editor/visor predeterminado.",
                ["RestoreToVanilla"] = "Restaurar Valores Vanilla",
                ["RestoreToVanilla_Desc"] = "Restaurar todas las opciones a los valores Vanilla.",
                ["ReloadToApply"] = "Recargar partida para aplicar los cambios.",
                ["ReloadToApply_Desc"] = "Recargar partida para aplicar los cambios.",
                ["NoLog"] = "Aún no existen registros.",
                ["PerXPop"] = "por {X} población",
                ["NotInGame"] = "No está en el juego",
            },
            ["fr-FR"] = new()
            {
                ["LogTab"] = "Log",
                ["NameText"] = "Mod Name",
                ["NameText_Desc"] = "The name of the mod.",
                ["VersionText"] = "Mod Version",
                ["VersionText_Desc"] = "The current version of the mod.",
                ["AuthorText"] = "Author",
                ["AuthorText_Desc"] = "The author of the mod.",
                ["BMaCLink_Desc"] = "Support the author.",
                ["Discord_Desc"] =
                    "Feedback/Suggestions for the mod in the Cities: Skylines Modding Server.",
                ["OpenLog"] = "Open Log File",
                ["OpenLog_Desc"] = "Open the log file in your default <.log> file viewer/editor.",
                ["RestoreToVanilla"] = "Restore Vanilla Values",
                ["RestoreToVanilla_Desc"] = "Restore all options to vanilla values.",
                ["ReloadToApply"] = "Reload save to apply changes.",
                ["ReloadToApply_Desc"] = "Reload save to apply changes.",
                ["NoLog"] = "No log entries yet.",
                ["PerXPop"] = "per {X} population",
                ["NotInGame"] = "Not in game",
            },
            ["it-IT"] = new()
            {
                ["LogTab"] = "Log",
                ["NameText"] = "Mod Name",
                ["NameText_Desc"] = "The name of the mod.",
                ["VersionText"] = "Mod Version",
                ["VersionText_Desc"] = "The current version of the mod.",
                ["AuthorText"] = "Author",
                ["AuthorText_Desc"] = "The author of the mod.",
                ["BMaCLink_Desc"] = "Support the author.",
                ["Discord_Desc"] =
                    "Feedback/Suggestions for the mod in the Cities: Skylines Modding Server.",
                ["OpenLog"] = "Open Log File",
                ["OpenLog_Desc"] = "Open the log file in your default <.log> file viewer/editor.",
                ["RestoreToVanilla"] = "Restore Vanilla Values",
                ["RestoreToVanilla_Desc"] = "Restore all options to vanilla values.",
                ["ReloadToApply"] = "Reload save to apply changes.",
                ["ReloadToApply_Desc"] = "Reload save to apply changes.",
                ["NoLog"] = "No log entries yet.",
                ["PerXPop"] = "per {X} population",
                ["NotInGame"] = "Not in game",
            },
            ["ja-JP"] = new()
            {
                ["LogTab"] = "Log",
                ["NameText"] = "Mod Name",
                ["NameText_Desc"] = "The name of the mod.",
                ["VersionText"] = "Mod Version",
                ["VersionText_Desc"] = "The current version of the mod.",
                ["AuthorText"] = "Author",
                ["AuthorText_Desc"] = "The author of the mod.",
                ["BMaCLink_Desc"] = "Support the author.",
                ["Discord_Desc"] =
                    "Feedback/Suggestions for the mod in the Cities: Skylines Modding Server.",
                ["OpenLog"] = "Open Log File",
                ["OpenLog_Desc"] = "Open the log file in your default <.log> file viewer/editor.",
                ["RestoreToVanilla"] = "Restore Vanilla Values",
                ["RestoreToVanilla_Desc"] = "Restore all options to vanilla values.",
                ["ReloadToApply"] = "Reload save to apply changes.",
                ["ReloadToApply_Desc"] = "Reload save to apply changes.",
                ["NoLog"] = "No log entries yet.",
                ["PerXPop"] = "per {X} population",
                ["NotInGame"] = "Not in game",
            },
            ["ko-KR"] = new()
            {
                ["LogTab"] = "Log",
                ["NameText"] = "Mod Name",
                ["NameText_Desc"] = "The name of the mod.",
                ["VersionText"] = "Mod Version",
                ["VersionText_Desc"] = "The current version of the mod.",
                ["AuthorText"] = "Author",
                ["AuthorText_Desc"] = "The author of the mod.",
                ["BMaCLink_Desc"] = "Support the author.",
                ["Discord_Desc"] =
                    "Feedback/Suggestions for the mod in the Cities: Skylines Modding Server.",
                ["OpenLog"] = "Open Log File",
                ["OpenLog_Desc"] = "Open the log file in your default <.log> file viewer/editor.",
                ["RestoreToVanilla"] = "Restore Vanilla Values",
                ["RestoreToVanilla_Desc"] = "Restore all options to vanilla values.",
                ["ReloadToApply"] = "Reload save to apply changes.",
                ["ReloadToApply_Desc"] = "Reload save to apply changes.",
                ["NoLog"] = "No log entries yet.",
                ["PerXPop"] = "per {X} population",
                ["NotInGame"] = "Not in game",
            },
            ["pl-PL"] = new()
            {
                ["LogTab"] = "Logi",
                ["NameText"] = "Nazwa",
                ["NameText_Desc"] = "Nazwa moda.",
                ["VersionText"] = "Wersja",
                ["VersionText_Desc"] = "Aktualna wersja moda.",
                ["AuthorText"] = "Autor",
                ["AuthorText_Desc"] = "Autor moda.",
                ["BMaCLink_Desc"] = "Wesprzyj autora.",
                ["Discord_Desc"] = "Opinie/sugestie o modzie na serwerze Cities: Skylines Modding.",
                ["OpenLog"] = "Otwórz plik logów",
                ["OpenLog_Desc"] =
                    "Otwórz plik logów w domyślnym edytorze/przeglądarce plików <.log>.",
                ["RestoreToVanilla"] = "Przywróć domyślne wartości",
                ["RestoreToVanilla_Desc"] = "Przywróci wszystkie opcje do domyślnych wartości.",
                ["ReloadToApply"] = "Wczytaj ponownie zapis, aby zastosować zmiany",
                ["ReloadToApply_Desc"] = "Wczyta ponownie zapis, aby zastosować zmiany.",
                ["NoLog"] = "Brak wpisów dziennika.",
                ["PerXPop"] = "na {X} populacji",
                ["NotInGame"] = "Nie w grze",
            },
            ["pt-BR"] = new()
            {
                ["LogTab"] = "Log",
                ["NameText"] = "Mod Name",
                ["NameText_Desc"] = "The name of the mod.",
                ["VersionText"] = "Mod Version",
                ["VersionText_Desc"] = "The current version of the mod.",
                ["AuthorText"] = "Author",
                ["AuthorText_Desc"] = "The author of the mod.",
                ["BMaCLink_Desc"] = "Support the author.",
                ["Discord_Desc"] =
                    "Feedback/Suggestions for the mod in the Cities: Skylines Modding Server.",
                ["OpenLog"] = "Open Log File",
                ["OpenLog_Desc"] = "Open the log file in your default <.log> file viewer/editor.",
                ["RestoreToVanilla"] = "Restore Vanilla Values",
                ["RestoreToVanilla_Desc"] = "Restore all options to vanilla values.",
                ["ReloadToApply"] = "Reload save to apply changes.",
                ["ReloadToApply_Desc"] = "Reload save to apply changes.",
                ["NoLog"] = "No log entries yet.",
                ["PerXPop"] = "per {X} population",
                ["NotInGame"] = "Not in game",
            },
            ["ru-RU"] = new()
            {
                ["LogTab"] = "Log",
                ["NameText"] = "Mod Name",
                ["NameText_Desc"] = "The name of the mod.",
                ["VersionText"] = "Mod Version",
                ["VersionText_Desc"] = "The current version of the mod.",
                ["AuthorText"] = "Author",
                ["AuthorText_Desc"] = "The author of the mod.",
                ["BMaCLink_Desc"] = "Support the author.",
                ["Discord_Desc"] =
                    "Feedback/Suggestions for the mod in the Cities: Skylines Modding Server.",
                ["OpenLog"] = "Open Log File",
                ["OpenLog_Desc"] = "Open the log file in your default <.log> file viewer/editor.",
                ["RestoreToVanilla"] = "Restore Vanilla Values",
                ["RestoreToVanilla_Desc"] = "Restore all options to vanilla values.",
                ["ReloadToApply"] = "Reload save to apply changes.",
                ["ReloadToApply_Desc"] = "Reload save to apply changes.",
                ["NoLog"] = "No log entries yet.",
                ["PerXPop"] = "per {X} population",
                ["NotInGame"] = "Not in game",
            },
            ["zh-HANS"] = new()
            {
                ["LogTab"] = "日志",
                ["NameText"] = "模组名称",
                ["NameText_Desc"] = "模组的名称。",
                ["VersionText"] = "模组版本",
                ["VersionText_Desc"] = "模组当前版本。",
                ["AuthorText"] = "作者",
                ["AuthorText_Desc"] = "模组的作者。",
                ["BMaCLink_Desc"] = "赞助作者。",
                ["Discord_Desc"] = "在 Cities: Skylines Modding 频道中反馈/提出建议。",
                ["OpenLog"] = "打开日志文件",
                ["OpenLog_Desc"] = "在默认的 <.log> 文件查看器/编辑器中打开日志文件。",
                ["RestoreToVanilla"] = "还原至原版值",
                ["RestoreToVanilla_Desc"] = "将所有数值还原到原版。",
                ["ReloadToApply"] = "重新加载存档以应用更改。",
                ["ReloadToApply_Desc"] = "重新加载存档以应用更改。",
                ["NoLog"] = "暂无日志条目。",
                ["PerXPop"] = "每 {X} 人口",
                ["NotInGame"] = "未在游戏",
            },
            ["zh-HANT"] = new()
            {
                ["LogTab"] = "Log",
                ["NameText"] = "Mod Name",
                ["NameText_Desc"] = "The name of the mod.",
                ["VersionText"] = "Mod Version",
                ["VersionText_Desc"] = "The current version of the mod.",
                ["AuthorText"] = "Author",
                ["AuthorText_Desc"] = "The author of the mod.",
                ["BMaCLink_Desc"] = "Support the author.",
                ["Discord_Desc"] =
                    "Feedback/Suggestions for the mod in the Cities: Skylines Modding Server.",
                ["OpenLog"] = "Open Log File",
                ["OpenLog_Desc"] = "Open the log file in your default <.log> file viewer/editor.",
                ["RestoreToVanilla"] = "Restore Vanilla Values",
                ["RestoreToVanilla_Desc"] = "Restore all options to vanilla values.",
                ["ReloadToApply"] = "Reload save to apply changes.",
                ["ReloadToApply_Desc"] = "Reload save to apply changes.",
                ["NoLog"] = "No log entries yet.",
                ["PerXPop"] = "per {X} population",
                ["NotInGame"] = "Not in game",
            },
        };

        public static Dictionary<string, string> CommonLocale()
        {
            string langId = GameManager.instance.localizationManager.activeLocaleId;
            if (!CommonLocales.TryGetValue(langId, out var translations))
                translations = CommonLocales["en-US"];

            return new()
            {
                [GetOptionsTabLocaleId("GeneralTab")] = Translate("Options.SECTION[General]"),
                [GetOptionsTabLocaleId("AboutTab")] = Translate("Options.SECTION[About]"),
                [GetOptionsTabLocaleId("LogTab")] = translations["LogTab"],
                [GetOptionsGroupLocaleId("GeneralGroup")] = Translate("Options.SECTION[General]"),

                [GetOptionsLabelLocaleId("NameText")] = translations["NameText"],
                [GetOptionsDescLocaleId("NameText")] = translations["NameText_Desc"],

                [GetOptionsLabelLocaleId("VersionText")] = translations["VersionText"],
                [GetOptionsDescLocaleId("VersionText")] = translations["VersionText_Desc"],

                [GetOptionsLabelLocaleId("AuthorText")] = translations["AuthorText"],
                [GetOptionsDescLocaleId("AuthorText")] = translations["AuthorText_Desc"],

                [GetOptionsLabelLocaleId("BMaCLink")] = Translate(
                    "Menu.ASSET_EXTERNAL_LINK_TYPE[buymeacoffee]"
                ),
                [GetOptionsDescLocaleId("BMaCLink")] = translations["BMaCLink_Desc"],

                [GetOptionsLabelLocaleId("Discord")] = Translate(
                    "Menu.ASSET_EXTERNAL_LINK_TYPE[discord]"
                ),
                [GetOptionsDescLocaleId("Discord")] = translations["Discord_Desc"],

                [GetOptionsLabelLocaleId("OpenLog")] = translations["OpenLog"],
                [GetOptionsDescLocaleId("OpenLog")] = translations["OpenLog_Desc"],

                [GetOptionsLabelLocaleId("RestoreToVanilla")] = translations["RestoreToVanilla"],
                [GetOptionsDescLocaleId("RestoreToVanilla")] = translations[
                    "RestoreToVanilla_Desc"
                ],

                [GetOptionsLabelLocaleId("ReloadToApply")] = translations["ReloadToApply"],
                [GetOptionsDescLocaleId("ReloadToApply")] = translations["ReloadToApply_Desc"],

                [$"{Id}.Mod.NoLog"] = translations["NoLog"],
                [$"{Id}.Mod.DefaultText"] = Translate("Options.INTERFACE_STYLE[default]"),
                [$"{Id}.Mod.PerXPop"] = translations["PerXPop"],
                [$"{Id}.Mod.NotInGame"] = translations["NotInGame"],
            };
        }
    }
}
