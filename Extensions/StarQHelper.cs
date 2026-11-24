using System;
using System.Collections.Generic;
using Colossal.Logging;
using StarQ.Shared.Extensions;

public static class StarQHelper
{
    public static string Id = "";
    public static string ModName = "";

    public static (bool, Exception) Init(
        string modId,
        string modName,
        ILog log,
        Func<Dictionary<string, string>> getReplacements = null,
        Action addLocale = null
    )
    {
        try
        {
            Id = modId;
            ModName = modName;
            LogHelper.Init(log);
            LocaleHelper.Init(getReplacements, addLocale);
        }
        catch (Exception e)
        {
            return (false, e);
        }
        return (true, null);
    }

    public static void SendDebugNotification(bool enable)
    {
#if DEBUG
        Game.PSI.NotificationSystem.Push($"init-debug-{Id}", ModName, $"DEV build running");
#else
        if (LogHelper.ReleaseDebug)
            if (enable)
                Game.PSI.NotificationSystem.Push(
                    $"init-debug-{Id}",
                    ModName,
                    $"Debug Logging enabled"
                );
            else
                Game.PSI.NotificationSystem.Pop($"init-debug-{Id}");
#endif
    }
}
