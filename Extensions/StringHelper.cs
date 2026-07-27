using System;
using System.Collections.Generic;

namespace StarQ.Shared.Extensions
{
    public class StringHelper
    {
        public static string FormatSize(ulong bytes)
        {
            double sizeInKB = bytes / 1024.0;
            double sizeInMB = sizeInKB / 1024.0;
            double sizeInGB = sizeInMB / 1024.0;

            if (sizeInGB >= 1)
                return $"{sizeInGB:0.##} GB";
            else if (sizeInMB >= 1)
                return $"{sizeInMB:0.##} MB";
            else
                return $"{sizeInKB:0.##} KB";
        }

        public static string FormatTime(long seconds, string suffix = " old")
        {
            if (seconds < 0)
                seconds = 0;

            TimeSpan ts = TimeSpan.FromSeconds(seconds);
            return FormatTime(ts, suffix);
        }

        public static string FormatTime(float seconds, string suffix = " old")
        {
            if (seconds < 0)
                seconds = 0;

            TimeSpan ts = TimeSpan.FromSeconds(seconds);
            return FormatTime(ts, suffix);
        }

        public static string FormatTime(TimeSpan ts, string suffix = " old")
        {
            var parts = new List<string>();

            if (ts.Days > 0)
                parts.Add($"{ts.Days} day{(ts.Days != 1 ? "s" : "")}");
            if (ts.Hours > 0)
                parts.Add($"{ts.Hours} hour{(ts.Hours != 1 ? "s" : "")}");
            if (ts.Minutes > 0)
                parts.Add($"{ts.Minutes} minute{(ts.Minutes != 1 ? "s" : "")}");
            if (ts.Seconds > 0 || parts.Count == 0)
                parts.Add($"{ts.Seconds} second{(ts.Seconds != 1 ? "s" : "")}");

            return string.Join(", ", parts) + suffix;
        }
    }
}
