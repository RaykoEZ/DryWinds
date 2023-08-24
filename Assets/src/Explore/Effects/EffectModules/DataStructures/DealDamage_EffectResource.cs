using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "DealDamage_", menuName = "Curry/Effects/DealDamage", order = 1)]
    public class DealDamage_EffectResource : BaseEffectResource
    {
        [SerializeField] DealDamageTo m_dealDamage = default;
        public DealDamageTo DamageModule => m_dealDamage;

        public override void Activate(GameStateContext context)
        {
            m_dealDamage?.ApplyEffect(context.Player);
        }
    }
}
