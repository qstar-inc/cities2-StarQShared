using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        DEVD,
    }

    public static class LogHelper
    {
        private static string _id = "Unknown";
        private static ILog _log;
        private static string _logPath;

        private static string _logText = "";
        private static DateTime _lastReadTime = DateTime.MinValue;

        private static readonly Regex PrefixRegex = new(
            @"^\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},\d{3}\] \[[A-Z]+\]\s*",
            RegexOptions.Multiline | RegexOptions.Compiled
        );

        public static LocalizedString LogText => LocalizedString.Id(_logText);

        public static void Init(string id, ILog log)
        {
            _id = id;
            _log = log;
            _logPath = Path.Combine(EnvPath.kUserDataPath, "Logs", $"{_id}.log");

            _logText = $"{_id}.Mod.NoLog";

            UpdateLogText();
            SendLog($"Starting {_id} at {DateTime.Now}", LogLevel.DEV);
        }

        public static void SendLog(string message, LogLevel level = LogLevel.Info)
        {
            if (_log == null)
                return;

            string text = message ?? "null";

            switch (level)
            {
                case LogLevel.Verbose:
                    _log.Verbose(text);
                    break;
                case LogLevel.Trace:
                    _log.Trace(text);
                    break;
                case LogLevel.Debug:
                    _log.Debug(text);
                    break;
                case LogLevel.Info:
                    _log.Info(text);
                    break;
                case LogLevel.Warn:
                    _log.Warn(text);
                    break;
                case LogLevel.Error:
                    _log.Error(text);
                    break;
                case LogLevel.Critical:
                    _log.Critical(text);
                    break;
                case LogLevel.Fatal:
                    _log.Fatal(text);
                    break;
                case LogLevel.Emergency:
                    _log.Emergency(text);
                    break;
                case LogLevel.DEV:
#if DEBUG
                    _log.Info(text);
                    break;
#else
                    return;
#endif
                case LogLevel.DEVD:
#if DEBUG
                    _log.Info(text);
#else
                    _log.Debug(text);
#endif
                    break;
                default:
                    _log.Info(text);
                    break;
            }

            UpdateLogText();
        }

        private static void UpdateLogText()
        {
            try
            {
                if (!File.Exists(_logPath))
                    return;

                DateTime writeTime = File.GetLastWriteTimeUtc(_logPath);

                if (writeTime <= _lastReadTime)
                    return;

                _lastReadTime = writeTime;

                string original = File.ReadAllText(_logPath);

                _logText = PrefixRegex.Replace(original, "");
            }
            catch (Exception e)
            {
                try
                {
                    _log?.Error(e);
                }
                catch { }
            }
        }

        public static void OpenLogFile() => Task.Run(() => Process.Start(_logPath));

        public static void SendLog(bool boolean, LogLevel level = LogLevel.Info) =>
            SendLog($"{boolean}", level);

        public static void SendLog(int integer, LogLevel level = LogLevel.Info) =>
            SendLog($"{integer}", level);

        public static void SendLog(float floatN, LogLevel level = LogLevel.Info) =>
            SendLog($"{floatN}", level);

        public static void SendLog(Exception exception, LogLevel level = LogLevel.Info)
        {
            if (exception is NullReferenceException)
            {
                SendLog("NullReferenceException: " + exception.Message, LogLevel.Error);
                SendLog(exception.StackTrace ?? "No stack trace available", LogLevel.Info);
                return;
            }

            SendLog(exception.ToString(), level);
        }

        public static bool CheckNull(
            Object obj,
            string text,
            string reason = "IDK how...",
            LogLevel level = LogLevel.Error
        )
        {
            if (obj == null)
            {
                SendLog($"{text} is null. {reason}", level);
                return true;
            }
            return false;
        }
    }
}
