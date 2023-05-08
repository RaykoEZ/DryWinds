using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Assault_CardResource : BaseCardResourceContainer
    {
        [SerializeField] DealDamage_EffectResource m_dealDamage = default;
        public DealDamageTo DamageModule => m_dealDamage.DamageModule;
    }
}
