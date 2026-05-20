using System.Collections.Generic;
using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Prefabs;
using Unity.Entities;

namespace StarQ.Shared.Extensions
{
    public partial class PrefabHelper : GameSystemBase
    {
        protected override void OnUpdate() { }

        public static Dictionary<Entity, string> prefabNames = new();

        public static string GetPrefabName(Entity selectedPrefab)
        {
            if (prefabNames.TryGetValue(selectedPrefab, out string name))
                return name;

            name = WorldHelper.PrefabSystem.GetPrefabName(selectedPrefab);

            prefabNames[selectedPrefab] = name;
            return name;
        }

        public static bool TryGetEntity(PrefabID prefabID, out Entity prefabEntity)
        {
            prefabEntity = Entity.Null;

            if (
                WorldHelper.PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefabBase)
                && WorldHelper.PrefabSystem.TryGetEntity(prefabBase, out prefabEntity)
            )
                return true;

            return false;
        }

        public static bool TryGetPrefab(
            string pType,
            string pName,
            Colossal.Hash128 pGuid,
            out PrefabBase prefabBase
        )
        {
            PrefabID prefabID = new(pType, pName, pGuid);

            if (WorldHelper.PrefabSystem.TryGetPrefab(prefabID, out prefabBase))
                return true;

            prefabID = new(pType, pName, default);

            if (WorldHelper.PrefabSystem.TryGetPrefab(prefabID, out prefabBase))
                return true;

            if (TryGetPrefabByType(pType, pName, out prefabBase))
                return true;

            LogHelper.SendLog($"{pType}:{pName} not found");
            return false;
        }

        public static bool TryGetPrefab(
            string pType,
            string pName,
            string pGuid,
            out PrefabBase prefabBase
        )
        {
            if (pGuid == "" | pGuid == null | pGuid == string.Empty)
                return TryGetPrefab(pType, pName, (Colossal.Hash128)(default), out prefabBase);

            return TryGetPrefab(pType, pName, Colossal.Hash128.Parse(pGuid), out prefabBase);
        }

        public static bool TryGetPrefab(string pType, string pName, out PrefabBase prefabBase) =>
            TryGetPrefab(pType, pName, (Colossal.Hash128)(default), out prefabBase);

        public static bool TryGetPrefabByType(string pType, string pName, out PrefabBase prefabBase)
        {
            var pm = AssetDatabase.global.GetAssets<PrefabAsset>();
            Dictionary<string, PrefabBase> prefabAssets = new();
            foreach (var pmItem in pm)
            {
                prefabBase = pmItem.GetInstance<PrefabBase>();

                if (pType == nameof(BuildingPrefab) && prefabBase is not BuildingPrefab)
                    continue;

                if (pType == nameof(StaticObjectPrefab) && prefabBase is not StaticObjectPrefab)
                    continue;

                if (prefabBase.name == pName)
                    return true;
            }

            prefabBase = null;
            return false;
        }

        public static bool TryGetEntity(
            string pType,
            string pName,
            Colossal.Hash128 pGuid,
            out Entity prefabEntity
        )
        {
            prefabEntity = Entity.Null;

            PrefabID prefabID = new(pType, pName, pGuid);

            if (
                WorldHelper.PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefabBase)
                && WorldHelper.PrefabSystem.TryGetEntity(prefabBase, out prefabEntity)
            )
                return true;

            prefabID = new(pType, pName, default);

            if (
                WorldHelper.PrefabSystem.TryGetPrefab(prefabID, out prefabBase)
                && WorldHelper.PrefabSystem.TryGetEntity(prefabBase, out prefabEntity)
            )
                return true;

            return false;
        }

        public static bool TryFindPrefabRef(Entity entity, out Entity prefabRef)
        {
            prefabRef = Entity.Null;

            if (WorldHelper.EntityManager.TryGetComponent(entity, out PrefabRef prefabRefData))
            {
                prefabRef = prefabRefData.m_Prefab;
                return true;
            }
            return false;
        }
    }
}
