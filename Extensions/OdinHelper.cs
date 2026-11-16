using System;
using Colossal.OdinSerializer;

namespace StarQ.Shared.Extensions
{
    public class OdinLogHelperLogger : ILogger
    {
        public void LogWarning(string warning)
        {
            LogHelper.SendLog($"[Odin Warning] {warning}");
        }

        public void LogError(string error)
        {
            LogHelper.SendLog($"[Odin Error] {error}");
        }

        public void LogException(Exception exception)
        {
            LogHelper.SendLog($"[Odin Exception] {exception}");
        }
    }
}
