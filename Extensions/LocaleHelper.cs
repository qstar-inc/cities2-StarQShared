using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Colossal;
using Colossal.Json;
using Colossal.Localization;
using Game.SceneFlow;

namespace StarQ.Shared.Extensions
{
    public partial class LocaleHelper
    {
        private static string Id;
        private static string ModName;
        private static Func<Dictionary<string, string>> GetReplacements;
        private static Action AddLocale;
        private static LocalizationManager localizationManager;

        public static Dictionary<string, string> toUpdate = new();

        public static void Init(
            string modId,
            string modName = null,
            Func<Dictionary<string, string>> getReplacements = null,
            Action addLocale = null
        )
        {
            Id = modId;
            ModName = modName;
            GetReplacements = getReplacements;
            AddLocale = addLocale;
            localizationManager = GameManager.instance.localizationManager;
            localizationManager.onActiveDictionaryChanged += OnActiveDictionaryChanged;
            foreach (var item in new LocaleHelper($"{Id}.Locale.json").GetAvailableLanguages())
                localizationManager.AddSource(item.LocaleId, item);

            LogHelper.SendLog("Initing LocaleHelper", LogLevel.DEV);
        }

        private readonly Dictionary<string, Dictionary<string, string>> _locale;
        private static readonly Dictionary<string, string> replacedStrings = new();

        public LocaleHelper(string dictionaryResourceName)
        {
            var assembly = GetType().Assembly;

            _locale = new Dictionary<string, Dictionary<string, string>>
            {
                [string.Empty] = GetDictionaryEmbedded(dictionaryResourceName),
            };

            foreach (var name in assembly.GetManifestResourceNames())
            {
                if (
                    name == dictionaryResourceName
                    || !name.Contains(
                        Path.GetFileNameWithoutExtension(dictionaryResourceName) + "."
                    )
                )
                    continue;

                var key = Path.GetFileNameWithoutExtension(name);

                _locale[key[(key.LastIndexOf('.') + 1)..]] = GetDictionaryEmbedded(name);
            }

            Dictionary<string, string> GetDictionaryEmbedded(string resourceName)
            {
                try
                {
                    using var resourceStream = assembly.GetManifestResourceStream(resourceName);
                    if (resourceStream == null)
                        return new Dictionary<string, string>();

                    using var reader = new StreamReader(resourceStream, Encoding.UTF8);
                    JSON.MakeInto<Dictionary<string, string>>(
                        JSON.Load(reader.ReadToEnd()),
                        out var dictionary
                    );

                    return dictionary;
                }
                catch (Exception ex)
                {
                    LogHelper.SendLog($"Failed to load embedded locale '{resourceName}': {ex}");
                    return new Dictionary<string, string>();
                }
            }
        }

        public static string Translate(string id, string fallback = null)
        {
            if (localizationManager.activeDictionary.TryGetValue(id, out var result))
                return result;
            if (fallback != null)
                return fallback;
            return id;
        }

        public static void AddLocalization(string id, string value) => toUpdate[id] = value;

        public static string GetServiceName(string id) => Translate($"Services.NAME[{id}]");

        public static string GetSubserviceName(string id) => Translate($"SubServices.NAME[{id}]");

        public static string GetOptionsTabLocaleId(string id) => $"Options.TAB[{Id}.{Id}.Mod.{id}]";

        public static string GetOptionsGroupLocaleId(string id) =>
            $"Options.GROUP[{Id}.{Id}.Mod.{id}]";

        public static string GetOptionsLabelLocaleId(string id) =>
            $"Options.OPTION[{Id}.{Id}.Mod.Setting.{id}]";

        public static string GetOptionsDescLocaleId(string id) =>
            $"Options.OPTION_DESCRIPTION[{Id}.{Id}.Mod.Setting.{id}]";

        public static string VanillaLocaleSignValueRemover(
            string id,
            string value,
            string sign = ""
        )
        {
            string translation = Translate(id);

            if (translation.Contains("{SIGN}"))
                translation = translation.Replace("{SIGN}", sign);

            if (translation.Contains("{VALUE}"))
                translation = translation.Replace("{VALUE}", value);

            return translation;
        }

        public IEnumerable<DictionarySource> GetAvailableLanguages()
        {
            foreach (var item in _locale)
                yield return new DictionarySource(item.Key is "" ? "en-US" : item.Key, item.Value);
        }

        public class DictionarySource : IDictionarySource
        {
            private readonly Dictionary<string, string> _dictionary;

            public DictionarySource(string localeId, Dictionary<string, string> dictionary)
            {
                LocaleId = localeId;
                _dictionary = dictionary;
            }

            public string LocaleId { get; }

            public IEnumerable<KeyValuePair<string, string>> ReadEntries(
                IList<IDictionaryEntryError> errors,
                Dictionary<string, int> indexCounts
            ) => _dictionary;

            public void Unload() { }
        }

        public static void OnActiveDictionaryChanged()
        {
            UpdateDictionary();
            foreach (var item in toUpdate.ToArray())
            {
                localizationManager.activeDictionary.Add(item.Key, item.Value);
                toUpdate.Remove(item.Key);
            }
            UpdateDictionary2();
            foreach (var item in toUpdate.ToArray())
            {
                localizationManager.activeDictionary.Add(item.Key, item.Value);
                toUpdate.Remove(item.Key);
            }
        }

        public static void UpdateDictionary()
        {
            AddLocalization($"Options.SECTION[{Id}.{Id}.Mod]", ModName);
            //Dictionary<string, string> toUpdateX = new();

            Dictionary<string, string> commonReplacements = CommonLocale();
            AddLocale?.Invoke();
            foreach (var item in commonReplacements)
                toUpdate[item.Key] = item.Value;

            foreach (var item in replacedStrings)
                toUpdate[item.Key] = item.Value;
        }

        public static void UpdateDictionary2()
        {
            if (GetReplacements() == null)
                return;

            Dictionary<string, string> replacements = GetReplacements();

            Regex regex = new($@"(\{{{Regex.Escape(Id)}\.[\w.]+\}}+)", RegexOptions.Compiled);
            //Regex regex2 = new(@"^\s*\{VanillaLocale\.(.+?)\}\s*$", RegexOptions.Compiled);
            var entries = localizationManager.activeDictionary.entries;

            foreach (var entry in entries)
            {
                //                string newValue; // = entry.Value;
                //if (entry.Value.Contains("{VanillaLocale."))
                //{
                //    LogHelper.SendLog(entry.Key);
                //    LogHelper.SendLog(entry.Value);
                //    var vanillaMatch = regex2.Match(entry.Value);
                //    LogHelper.SendLog(vanillaMatch.Success);
                //    if (vanillaMatch.Success)
                //    {
                //        string vanillaKey = vanillaMatch.Groups[1].Value;
                //        LogHelper.SendLog(vanillaKey);
                //        newValue = Translate(vanillaKey);
                //        LogHelper.SendLog(newValue);
                //    }
                //}
                //else
                //{

                if (!entry.Key.Contains(Id))
                    continue;

                if (
                    !(
                        entry.Value.Contains($"{Id}.Replacement.")
                        || (entry.Value.Contains("{") && entry.Value.Contains("}"))
                    )
                )
                    continue;

                //LogHelper.SendLog($"Expand Start: {entry.Value}");
                string newValue = Expand(entry.Value, replacements, regex);

                if (newValue != entry.Value)
                {
                    replacedStrings[entry.Key] = entry.Value;
                    //LogHelper.SendLog($"Expand End: {entry.Value}, {newValue}");
                    toUpdate[entry.Key] = newValue;
                }
            }

            //foreach (var item in toUpdate)
            //    localizationManager.activeDictionary.Add(item.Key, item.Value);
            //{
            //    //if (!lm.activeDictionary.TryGetValue(item.Key, out var _))
            //    localizationManager.activeDictionary.Add(item.Key, item.Value);
            //    //else
            //    //    LogHelper.SendLog(
            //    //        $"Trying to add '{item.Key}: {item.Value}' locale when it already exists"
            //    //    );
            //}
        }

        static string Expand(string input, Dictionary<string, string> replacements, Regex regex)
        {
            string result = input;
            bool changed;
            int safety = 0;

            do
            {
                changed = false;
                result = regex.Replace(
                    result,
                    match =>
                    {
                        var key = match.Groups[1].Value.Trim('{', '}');
                        if (
                            replacements.TryGetValue(
                                key.Replace($"{Id}.Replacement.", ""),
                                out var replacement
                            )
                        )
                        {
                            changed = true;
                            return replacement;
                        }

                        if (
                            localizationManager.activeDictionary.TryGetValue(key, out var localized)
                        )
                        {
                            changed = true;
                            return localized;
                        }

                        return match.Value;
                    }
                );
                if (++safety > 10)
                {
                    LogHelper.SendLog($"Expand loop aborted. Key={Id}, input={input}");
                    break;
                }
            } while (changed && regex.IsMatch(result));

            return result;
        }
    }
}
