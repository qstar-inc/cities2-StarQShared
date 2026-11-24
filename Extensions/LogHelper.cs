using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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
        private static ILog log;
        private static string logPath;

        private static string logText = "";
        private static DateTime lastReadTime = DateTime.MinValue;

        private const int MaxLogLines = 200;
        private const string TruncationNotice =
            "Log file is too large, open the log file to read the remaining.";

        private static readonly Regex PrefixRegex = new(
            @"^\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},\d{3}\] \[[A-Z]+\]\s*",
            RegexOptions.Multiline | RegexOptions.Compiled
        );

        public static bool ReleaseDebug = false;
        public static LocalizedString LogText => LocalizedString.Id(logText);

        public static void Init(ILog log)
        {
            LogHelper.log = log;
            logPath = Path.Combine(EnvPath.kUserDataPath, "Logs", $"{StarQHelper.Id}.log");

            logText = $"{StarQHelper.Id}.NoLog";

            UpdateLogText();
            SendLog($"Starting {StarQHelper.Id} at {DateTime.Now}", LogLevel.DEV);
        }

        public static void SendLog(string message, LogLevel level = LogLevel.Info)
        {
            if (log == null)
                return;

            string text = message ?? "null";

            switch (level)
            {
                case LogLevel.Verbose:
                    log.Verbose(text);
                    break;
                case LogLevel.Trace:
                    log.Trace(text);
                    break;
                case LogLevel.Debug:
                    log.Debug(text);
                    break;
                case LogLevel.Info:
                    log.Info(text);
                    break;
                case LogLevel.Warn:
                    log.Warn(text);
                    break;
                case LogLevel.Error:
                    log.Error(text);
                    break;
                case LogLevel.Critical:
                    log.Critical(text);
                    break;
                case LogLevel.Fatal:
                    log.Fatal(text);
                    break;
                case LogLevel.Emergency:
                    log.Emergency(text);
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
                    if (ReleaseDebug)
                        log.Info(text);
                    else
                        log.Debug(text);
#endif
                    break;
                default:
                    log.Info(text);
                    break;
            }

            UpdateLogText();
        }

        private static void UpdateLogText()
        {
            try
            {
                if (!File.Exists(logPath))
                    return;

                DateTime writeTime = File.GetLastWriteTimeUtc(logPath);

                if (writeTime <= lastReadTime)
                    return;

                lastReadTime = writeTime;

                var lines = new List<string>(MaxLogLines);
                bool truncated = false;

                using (
                    var stream = new FileStream(
                        logPath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite
                    )
                )
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lines.Count >= MaxLogLines)
                        {
                            truncated = true;
                            break;
                        }
                        lines.Add(line);
                    }
                }

                string original = string.Join(Environment.NewLine, lines);
                string cleaned = PrefixRegex.Replace(original, "");

                logText = truncated ? cleaned + Environment.NewLine + TruncationNotice : cleaned;
            }
            catch (Exception) { }
        }

        public static void OpenLogFile()
        {
            if (!CheckLogFile())
                return;

            Task.Run(() =>
            {
                try
                {
                    Process.Start(logPath);
                }
                catch (Exception ex)
                {
                    SendLog(ex);
                }
            });
        }

        public static bool CheckLogFile() => !File.Exists(logPath);

        public static void SendLog(bool boolean, LogLevel level = LogLevel.Info) =>
            SendLog(boolean.ToString(), level);

        public static void SendLog(int integer, LogLevel level = LogLevel.Info) =>
            SendLog(integer.ToString(), level);

        public static void SendLog(float floatN, LogLevel level = LogLevel.Info) =>
            SendLog(floatN.ToString(), level);

        public static void SendLog(Exception exception, LogLevel level = LogLevel.Error)
        {
            StringBuilder sb = new();

            sb.AppendLine("");

            Exception current = exception;
            int depth = 0;
            while (current != null)
            {
                try
                {
                    sb.AppendLine($"[{depth}] {current.GetType().FullName}: {current.Message}");

                    if (!string.IsNullOrEmpty(current.Source))
                        sb.AppendLine($"  Source: {current.Source}");

                    if (!string.IsNullOrEmpty(current.StackTrace))
                        sb.AppendLine(TrimPath(current.StackTrace));
                }
                catch
                {
                    sb.AppendLine($"[{depth}] <failed to read exception data>");
                }

                current = current.InnerException;
                depth++;

                if (depth > 10)
                {
                    sb.AppendLine("... chain truncated after 10 levels ...");
                    break;
                }
            }

            SendLog(sb.ToString(), level);
        }

        public static void SendLog(StringBuilder sb, LogLevel level = LogLevel.Error) =>
            SendLog(sb.ToString(), level);

        public static bool CheckNull(
            object? obj,
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

        private static string TrimPath(string stackTrace)
        {
            if (string.IsNullOrEmpty(stackTrace) || string.IsNullOrEmpty(StarQHelper.Id))
                return stackTrace;

            var lines = stackTrace.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');

                string marker =
                    Path.DirectorySeparatorChar + StarQHelper.Id + Path.DirectorySeparatorChar;
                int idx = line.IndexOf(marker, StringComparison.Ordinal);
                if (idx >= 0)
                {
                    int inIdx = line.IndexOf(" in ", StringComparison.Ordinal);
                    if (inIdx >= 0)
                    {
                        string before = line[..(inIdx + 4)];
                        string pathPart = line[(inIdx + 4)..];
                        int pathMarkerIdx = pathPart.IndexOf(marker, StringComparison.Ordinal);
                        if (pathMarkerIdx >= 0)
                            pathPart = pathPart[(pathMarkerIdx + marker.Length)..];
                        line = before + pathPart;
                    }
                }

                lines[i] = FormatFrame(line);
            }

            return string.Join("\n", lines);
        }

        private static readonly Regex FrameRegex = new(
            @"at\s+(?:[\w.]+\.)*(?<method>[\w<>`]+)\s*\([^)]*\)\s*(?:\[[^\]]*\]\s*)?in\s+(?<path>.+)$",
            RegexOptions.Compiled
        );

        private static string FormatFrame(string line)
        {
            var match = FrameRegex.Match(line);
            if (!match.Success)
                return line;

            return $"  {match.Groups["method"].Value} in {match.Groups["path"].Value}";
        }
    }
}
