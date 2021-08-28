using UnityEngine;

namespace Curry.Game
{
    public class HealingItem : BaseItem
    {
        [SerializeField] protected float m_healAmount = default;
        public override bool ActivateEffect(BaseCharacter hit) 
        {
            bool ret = hit.CurrentStats.Stamina < hit.CurrentStats.MaxStamina;
            hit.OnHeal(m_healAmount);
            return ret;
        }
    }
}
