using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public class NpcSpawner : InteractableSpawner
    {
        [SerializeField] protected List<NpcTerritory> m_territories = default;

        public override GameObject Spawn(Transform parent = null)
        {
            GameObject ret = base.Spawn(parent);
            ret.GetComponent<BaseNpc>()?.SetupTerritory(m_territories);
            return ret;
        }
    }
}
