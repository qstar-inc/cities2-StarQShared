using System;
using System.Linq;
using Colossal.IO.AssetDatabase;
using Game.SceneFlow;

namespace StarQ.Shared.Extensions
{
    public class ModHelper
    {
        public static bool IsModActive(string modName)
        {
            var list = GameManager.instance.modManager.ListModsEnabled();
            if (list.Contains(modName))
                return true;

            for (int i = 0; i < list.Length; i++)
                if (list[i].StartsWith(modName))
                    return true;

            return false;
        }

        public static bool AddAfterActivePlaysetOrModStatusChanged(Action func)
        {
            try
            {
                var db = AssetDatabase<ParadoxMods>.instance;
                var ds = (ParadoxModsDataSource)db.dataSource;
                ds.onAfterActivePlaysetOrModStatusChanged += func;
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.SendLog(ex, LogLevel.Error);
                return false;
            }
        }
    }
}
