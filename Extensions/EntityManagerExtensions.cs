using Game.Common;
using Game.Prefabs;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;

namespace StarQ.Shared.Extensions
{
    public static class EntityManagerExtensions
    {
        public static void SetOrAdd<T>(this EntityManager em, Entity entity, T value)
            where T : unmanaged, IComponentData
        {
            if (!em.HasComponent<T>(entity))
                em.AddComponentData(entity, value);
            else
                em.SetComponentData(entity, value);
        }

        public static void SetAndUpdate<T>(this EntityManager em, Entity entity, T value)
            where T : unmanaged, IComponentData
        {
            em.SetAndUpdate(entity, entity, value);
        }

        public static void SetAndUpdate<T>(
            this EntityManager em,
            Entity entity,
            Entity toUpdateEntity,
            T value
        )
            where T : unmanaged, IComponentData
        {
            em.SetOrAdd(entity, value);
            em.AddComponent<Updated>(toUpdateEntity);
        }

        public static void AddAndUpdate<TAdd>(this EntityManager em, Entity entity, TAdd toAdd)
            where TAdd : unmanaged, IComponentData
        {
            em.AddComponentData(entity, toAdd);
            em.AddComponent<Updated>(entity);
        }

        public static void UpdateAllPlacedInstance(this EntityManager em, Entity prefabRef)
        {
            if (prefabRef == Entity.Null || !em.Exists(prefabRef))
                return;

            EntityQuery query = em.CreateEntityQuery(
                ComponentType.ReadOnly<PrefabRef>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>()
            );

            NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp);
            NativeArray<PrefabRef> refs = query.ToComponentDataArray<PrefabRef>(Allocator.Temp);

            NativeList<Entity> matches = new(entities.Length, Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                if (refs[i].m_Prefab == prefabRef)
                    matches.Add(entities[i]);
            }

            if (matches.Length == 0)
                return;

            em.AddComponent<Updated>(matches.AsArray());
        }
    }
}
