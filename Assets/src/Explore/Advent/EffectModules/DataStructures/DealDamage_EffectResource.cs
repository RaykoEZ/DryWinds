using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class DealDamage_EffectResource : BaseEffectResource
    {
        [SerializeField] DealDamageTo m_dealDamage = default;
        public DealDamageTo DamageModule => m_dealDamage;
    }
}
