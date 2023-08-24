using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Heal_", menuName = "Curry/Effects/Heal", order = 1)]
    public class Heal_EffectResource : BaseEffectResource
    {
        [SerializeField] HealingModule m_healing = default;
        public HealingModule Healing => m_healing;

        public override void Activate(GameStateContext context)
        {
            m_healing.ApplyEffect(context.Player);
        }
    }
}
