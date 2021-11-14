using UnityEngine;

namespace Curry.Game
{
    public class HealingItem : BaseItem
    {
        [SerializeField] protected float m_healAmount = default;
        protected override bool OnActivate(BaseCharacter hit) 
        {
            bool ret = hit.CurrentStats.Stamina < hit.CurrentStats.MaxStamina;
            hit.OnHeal(m_healAmount);
            return ret;
        }
    }
}
