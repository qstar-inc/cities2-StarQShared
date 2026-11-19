using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace StarQ.Shared.Extensions
{
    public class VariableHelper
    {
        public const string StarQ = "StarQ / Qoushik";
        public const string ServerID_CSM = "1024242828114673724";
        private const string DefaultCSMThread = "1530639133112930565";
        public static Action<string> OpenURL = url => Application.OpenURL($"{url}");
        public static Action OpenBMAC = () => OpenURL($"https://buymeacoffee.com/starq");
        public static Action OpenParadoxForum = () =>
            OpenURL($"https://forum.paradoxplaza.com/forum/threads/1930389/");

        public static void OpenStarQDiscord() => OpenURL($"https://discord.gg/U2PSjUzUuF");

        public static void OpenCSMDiscord(string group)
        {
            string channel = AppDomain
                .CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == group)
                ?.GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(a => a.Key == "CSMThread")
                ?.Value;

            if (string.IsNullOrEmpty(channel))
                channel = DefaultCSMThread;

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
    }
}
