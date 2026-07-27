using System;
using System.Threading.Tasks;
using Game;
using Game.Common;
using Game.Modding;
using Game.Notifications;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Tools;
using Game.UI;
using Game.UI.InGame;
using Unity.Entities;

namespace StarQ.Shared.Extensions
{
    public class WorldHelper
    {
        public static EntityManager EntityManager =>
            World.DefaultGameObjectInjectionWorld.EntityManager;

        public static ModManager ModManager => GameManager.instance.modManager;
        public static IconCommandSystem IconCommandSystem => GetSystem<IconCommandSystem>();
        public static ImageSystem ImageSystem => GetSystem<ImageSystem>();
        public static ModificationBarrier1 ModificationBarrier1 =>
            GetSystem<ModificationBarrier1>();
        public static ModificationEndBarrier ModificationEndBarrier =>
            GetSystem<ModificationEndBarrier>();
        public static NameSystem NameSystem => GetSystem<NameSystem>();
        public static PrefabSystem PrefabSystem => GetSystem<PrefabSystem>();
        public static ToolSystem ToolSystem => GetSystem<ToolSystem>();
        public static PrefabUISystem PrefabUISystem => GetSystem<PrefabUISystem>();

        public static T GetSystem<T>()
            where T : SystemBase =>
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<T>();

        public static bool IsGame => GameManager.instance.gameMode.IsGame();
        public static bool IsEditor => GameManager.instance.gameMode.IsEditor();
        public static bool IsGameOrEditor => GameManager.instance.gameMode.IsGameOrEditor();

        public static Task RunOnMainThreadAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            Colossal.Core.MainThreadDispatcher.RunOnMainThread(() =>
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }
    }
}
