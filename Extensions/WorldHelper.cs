using Game.Common;
using Game.Notifications;
using Game.Prefabs;
using Game.UI;
using Game.UI.InGame;
using Unity.Entities;

namespace StarQ.Shared.Extensions
{
    public class WorldHelper
    {
        public static EntityManager EntityManager =>
            World.DefaultGameObjectInjectionWorld.EntityManager;

        public static IconCommandSystem IconCommandSystem => GetSystem<IconCommandSystem>();
        public static ImageSystem ImageSystem => GetSystem<ImageSystem>();
        public static ModificationBarrier1 ModificationBarrier1 =>
            GetSystem<ModificationBarrier1>();
        public static ModificationEndBarrier ModificationEndBarrier =>
            GetSystem<ModificationEndBarrier>();
        public static NameSystem NameSystem => GetSystem<NameSystem>();
        public static PrefabSystem PrefabSystem => GetSystem<PrefabSystem>();
        public static PrefabUISystem PrefabUISystem => GetSystem<PrefabUISystem>();

        public static T GetSystem<T>()
            where T : SystemBase =>
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<T>();
    }
}
