using UnityEngine;
using System;
using System.Collections.Generic;
using Curry.Game;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "SpawnInRange_", menuName = "Curry/Effects/SpawnInRange", order = 1)]
    public class SpawnInRange_EffectResource : BaseEffectResource 
    {
        [SerializeField] SpawnInRange m_spawn;
        [SerializeField] List<PoolableBehaviour> m_toSpawn = default;
        public SpawnInRange SpawnModule => m_spawn;
        public override void Activate(GameStateContext context)
        {
            if (m_toSpawn.Count < 1) { return; }

            m_spawn?.ApplyEffect(context.Player.WorldPosition, m_toSpawn);

        }
    }
}
