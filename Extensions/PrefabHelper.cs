using System;
using System.Collections.Generic;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Prefabs;
using Game.UI.InGame;
using Unity.Collections;
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
                !WorldHelper.PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefabBase)
                || !WorldHelper.PrefabSystem.TryGetEntity(prefabBase, out prefabEntity)
            )
                return false;
            return true;
        }
    }
}
