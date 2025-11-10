using System;
using System.IO;
using System.Text.RegularExpressions;
using Colossal.Logging;
using Colossal.PSI.Environment;
using Game.UI.Localization;

namespace StarQ.Shared.Extensions
{
    public enum LogLevel
    {
        Verbose,
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Critical,
        Fatal,
        Emergency,
        DEV,
    }

    public class LogHelper
    {
        public static string Id;
        public static ILog log;
        public static string logPath;

        public static void Init(string _id, ILog _log)
        {
            Id = _id;
            log = _log;
            logText = $"{Id}.Mod.NoLog";
            logPath = $"{EnvPath.kUserDataPath}/Logs/{Id}.log";

            SendLog($"Starting {Id} at {DateTime.Now.ToLocalTime()}", LogLevel.DEV);
        }

        public static LocalizedString LogText => LocalizedString.Id(logText);
        private static string logText = "";
        private static bool logExists = false;

        public static void SendLog(string message, LogLevel level = LogLevel.Info)
        {
            switch (level)
            {
                case LogLevel.Verbose:
                    log.Verbose(message);
                    break;
                case LogLevel.Trace:
                    log.Trace(message);
                    break;
                case LogLevel.Debug:
                    log.Debug(message);
                    break;
                case LogLevel.Info:
                    log.Info(message);
                    break;
                case LogLevel.Warn:
                    log.Warn(message);
                    break;
                case LogLevel.Error:
                    log.Error(message);
                    break;
                case LogLevel.Critical:
                    log.Critical(message);
                    break;
                case LogLevel.Fatal:
                    log.Fatal(message);
                    break;
                case LogLevel.Emergency:
                    log.Emergency(message);
                    break;
                case LogLevel.DEV:
#if DEBUG
                    log.Info(message);
#endif
                    break;
                default:
                    log.Info(message);
                    break;
            }
            try
            {
                if (!logExists)
                    logExists = File.Exists(logPath);

                if (!logExists)
                    return;

                string oglogText = File.ReadAllText(logPath);
                logText = Regex.Replace(
                    oglogText,
                    @"^\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},\d{3}\] \[[A-Z]+\]\s*",
                    "",
                    RegexOptions.Multiline
                );
            }
            catch (Exception e)
            {
                log.Info(e);
            }
        }

        public static void SendLog(Exception exception, LogLevel level = LogLevel.Info) =>
            SendLog($"{exception}", level);

        public static void SendLog(bool boolean, LogLevel level = LogLevel.Info) =>
            SendLog($"{boolean}", level);

        public static void SendLog(int integer, LogLevel level = LogLevel.Info) =>
            SendLog($"{integer}", level);

        public static void SendLog(float floatN, LogLevel level = LogLevel.Info) =>
            SendLog($"{floatN}", level);
    }
}
