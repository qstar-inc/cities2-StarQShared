using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Colossal.IO.AssetDatabase;
using Colossal.PSI.Common;
using Colossal.PSI.PdxSdk;
using Game.SceneFlow;
using PDX.SDK.Contracts;
using PDX.SDK.Contracts.Service.Account;
using PDX.SDK.Contracts.Service.Account.Results;
using PDX.SDK.Contracts.Service.Mods.Enums;
using PDX.SDK.Contracts.Service.Mods.Interfaces;
using PDX.SDK.Contracts.Service.Mods.Models;
using PDX.SDK.Contracts.Service.Profile.Results;
using Steamworks;

namespace StarQ.Shared.Extensions
{
    public enum ModScanStatus
    {
        None,
        InProgress,
        Completed,
    }

    public class ModHelper
    {
        private static PdxSdkPlatform? m_Manager = null;
        private static readonly FieldInfo SDKContextField = typeof(PdxSdkPlatform).GetField(
            "m_SDKContext",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        private static IContext? m_Context = null;

        public static string UserName = string.Empty;
        public static string UserId = string.Empty;

        public static PdxSdkPlatform GetManager()
        {
            m_Manager ??= PlatformManager.instance.GetPSI<PdxSdkPlatform>("PdxSdk");

            return m_Manager;
        }

        public static IContext GetContext()
        {
            GetManager();
            m_Context ??= (IContext)SDKContextField.GetValue(m_Manager);

            return m_Context;
        }

        public static bool IsOnPublicBuild()
        {
            if (PlatformManager.instance.principalPlatformName != "Steamworks")
                return true;

            if (SteamApps.GetAppBuildId() == 23700737)
                return true;

            return false;
        }

        public static bool IsModActive(string modName)
        {
            var list = WorldHelper.ModManager.ListModsEnabled();
            if (list.Contains(modName))
                return true;

            for (int i = 0; i < list.Length; i++)
                if (list[i].StartsWith(modName))
                    return true;

            return false;
        }

        public static ExecutableAsset? GetModAssembly(string name)
        {
            foreach (ExecutableAsset executableAsset in ExecutableAsset.GetModAssets())
                if (executableAsset.name == name)
                    return executableAsset;

            return null;
        }

        public static bool AddAfterActivePlaysetOrModStatusChanged(Action func)
        {
            try
            {
                AssetDatabase<ParadoxMods>? db = AssetDatabase<ParadoxMods>.instance;
                ParadoxModsDataSource ds = (ParadoxModsDataSource)db.dataSource;
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

            IContext context = GetContext();

            if (SDKContextField == null || m_Manager == null)
                return Array.Empty<string>();

            if (context == null)
                return Array.Empty<string>();

            PDX.SDK.Contracts.Service.Mods.Results.IModRootsResult x =
                context.Mods.LocalFiles.GetModSourcesRootPaths();

            if (!x.Success)
                return Array.Empty<string>();

            foreach (ModSourceRoots root in x.ModRoots)
                if (root.Source == SourceType.PdxMods)
                    foreach (string p in root.RootPaths)
                        paths.Add(Path.GetFullPath(p).Replace("\\", "/"));

            return paths.ToArray();
        }

        public static string GetModPath<T>(T mod)
            where T : Game.Modding.IMod
        {
            if (
                WorldHelper.ModManager.TryGetExecutableAsset(mod, out var asset)
                && asset != null
                && asset.path != null
            )
                return Path.GetDirectoryName(asset.path);
            return "";
        }

        public static HashSet<ILocalPlaysetMod> GetLocalPlaysetMods()
        {
            IContext context = GetContext();

            PDX.SDK.Contracts.Service.Mods.Results.IListModsInPlaysetResult playsetResult = context
                .Mods.GetActivePlaysetEnabledMods()
                .Result;
            ILocalPlaysetMod[] modsResult = !playsetResult.Success
                ? Array.Empty<ILocalPlaysetMod>()
                : (playsetResult.Mods ?? Enumerable.Empty<ILocalPlaysetMod>())
                    .Where(m => !string.IsNullOrEmpty(m?.LocalData?.FolderAbsolutePath))
                    .ToArray();

            return modsResult != null
                ? new HashSet<ILocalPlaysetMod>(modsResult)
                : new HashSet<ILocalPlaysetMod>();
        }

        public static async Task GetLoggedInUserName()
        {
            IContext context = GetContext();

            IAccountStartupResult accountStartupResult = await context.Account.Startup();

            if (accountStartupResult == null || !accountStartupResult.IsLoggedIn)
            {
                LogHelper.SendLog($"Failed getting data, user not logged in.");
                return;
            }

            try
            {
                ISocialProfileResult socialProfileResult = await context
                    .Profile.Get()
                    .ConfigureAwait(false);

                if (
                    socialProfileResult != null
                    && socialProfileResult.Success
                    && socialProfileResult.Social != null
                    && socialProfileResult.Social.DisplayName != null
                )
                {
                    UserName = socialProfileResult.Social.DisplayName;
                    UserId = accountStartupResult.UserId;
                }
            }
            catch (Exception ex)
            {
                LogHelper.SendLog($"Failed getting username, probably not logged in.\n{ex}");
            }
        }

        public static async Task CacheLoggedInUserName()
        {
            if (UserName != string.Empty)
                return;

            await GetLoggedInUserName();
        }

        public static async Task<IModsUploadSupport.ModInfo?> GetModDetail(
            IModsUploadSupport.ModInfo mod
        )
        {
            PdxSdkPlatform manager = GetManager();

            try
            {
                IModsUploadSupport.ModInfo result = await manager.GetDetails(mod, null, null);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.SendLog(ex);
            }
            return null;
        }
    }
}
