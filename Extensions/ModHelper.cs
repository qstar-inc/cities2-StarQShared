using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Colossal.IO.AssetDatabase;
using Colossal.PSI.Common;
using Colossal.PSI.PdxSdk;
using Game.SceneFlow;
using PDX.SDK.Contracts;
using PDX.SDK.Contracts.Service.Mods.Models;
using TinyJson;

namespace StarQ.Shared.Extensions
{
    public class ModHelper
    {
        private static PdxSdkPlatform m_Manager;

        private static readonly FieldInfo SDKContextField = typeof(PdxSdkPlatform).GetField(
            "m_SDKContext",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        public static bool IsModActive(string modName)
        {
            var list = GameManager.instance.modManager.ListModsEnabled();
            if (list.Contains(modName))
                return true;

            for (int i = 0; i < list.Length; i++)
                if (list[i].StartsWith(modName))
                    return true;

            return false;
        }

        public static bool AddAfterActivePlaysetOrModStatusChanged(Action func)
        {
            try
            {
                var db = AssetDatabase<ParadoxMods>.instance;
                var ds = (ParadoxModsDataSource)db.dataSource;
                ds.onAfterActivePlaysetOrModStatusChanged += func;
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.SendLog(ex, LogLevel.Error);
                return false;
            }
        }

        public static string[] GetPDXModsPath()
        {
            List<string> paths = new();

            m_Manager = PlatformManager.instance.GetPSI<PdxSdkPlatform>("PdxSdk");
            IContext context = (IContext)SDKContextField.GetValue(m_Manager);

            if (SDKContextField == null || m_Manager == null)
                return Array.Empty<string>();

            if (context == null)
                return Array.Empty<string>();

            PDX.SDK.Contracts.Service.Mods.Results.IModRootsResult x = context
                .Mods.LocalFiles.GetModSourcesRootFolders()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            if (!x.Success)
                return Array.Empty<string>();

            foreach (ModSourceRoots root in x.ModRoots)
                if (root.Source == SourceType.PdxMods)
                    foreach (string p in root.RootPaths)
                        paths.Add(Path.GetFullPath(p).Replace("\\", "/"));

            return paths.ToArray();
        }
    }
}
