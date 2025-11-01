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
        public static Action<string> OpenURL = url => Application.OpenURL($"{url}");
        public static Action OpenBMAC = () => OpenURL($"https://buymeacoffee.com/starq");
        public static Action<string> OpenDiscord = channel =>
            OpenURL($"https://discord.com/channels/1024242828114673724/{channel}");

        public static string AddDevSuffix(string version)
        {
#if DEBUG
            return $"{version} - DEV";
#else
            return version;
#endif
        }

        public static Task OpenLog(string id) { return Task.Run(() =>
                    Process.Start($"{EnvPath.kUserDataPath}/Logs/{id}.log")
                );}

        public static bool CheckLog(string id) {
            try
            {
                return !System.IO.File.Exists(
                    $"{EnvPath.kUserDataPath}/Logs/{id}.log"
                );
            }
            catch (Exception e)
            {
                LogHelper.SendLog(e);
                return true;
            }
        }
    }
}
