using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Colossal.PSI.Environment;
using UnityEngine;

namespace StarQ.Shared.Extensions
{
    public class VariableHelper
    {
        public static string StarQ = "StarQ / Qoushik";
        public static string ServerID_CSM = "1024242828114673724";
        public static Action<string> OpenURL = url => Application.OpenURL($"{url}");
        public static Action OpenBMAC = () => OpenURL($"https://buymeacoffee.com/starq");
        public static Action OpenParadoxForum = () =>
            OpenURL($"https://forum.paradoxplaza.com/forum/threads/1930389/");

        //public static Action<string> OpenDiscord = channel =>
        //    OpenURL($"https://discord.com/channels/1024242828114673724/{channel}");

        public static void OpenDiscord(string group)
        {
            string channel = group switch
            {
                "SimpleModCheckerPlus" => "1518525133730353273",
                "AssetUIManager" => "1518565970682052628",
                "AdvancedBuildingControl" => "1464858378747645983",
                "AssetMigrationUtility" => "1518597681939878141",
                "RailVehicleTuner" => "1490730915717189844",
                "MeshStatsViewer" => "1519761704991330435",
                _ => "1530639133112930565",
            };

            OpenURL($"https://discord.com/channels/{ServerID_CSM}/{channel}");
        }

        public static string AddDevSuffix(string version)
        {
#if DEBUG
            return $"{version} - DEV";
#else
            return version;
#endif
        }

        public static Task OpenLog(string id)
        {
            return Task.Run(() => Process.Start($"{EnvPath.kUserDataPath}/Logs/{id}.log"));
        }

        public static bool CheckLog(string id)
        {
            try
            {
                return !System.IO.File.Exists($"{EnvPath.kUserDataPath}/Logs/{id}.log");
            }
            catch (Exception e)
            {
                LogHelper.SendLog(e);
                return true;
            }
        }
    }
}
