using UnityEngine;
using System;

namespace Curry.Explore
{
    [Serializable]
    public class SpawnInRange_EffectResource : BaseEffectResource 
    {
        [SerializeField] SpawnInRange m_spawn;
        public SpawnInRange SpawnModule => m_spawn;
    }
}
