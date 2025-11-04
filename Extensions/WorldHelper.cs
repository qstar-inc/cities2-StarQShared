using System;
using System.Linq;
using Game.Prefabs;
using Game.SceneFlow;
using Unity.Entities;

namespace StarQ.Shared.Extensions
{
    public class WorldHelper
    {
        public static EntityManager entityManager =>
            World.DefaultGameObjectInjectionWorld.EntityManager;

        public static PrefabSystem prefabSystem => GetSystem<PrefabSystem>();

        public static T GetSystem<T>()
            where T : SystemBase =>
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<T>();
    }
}
