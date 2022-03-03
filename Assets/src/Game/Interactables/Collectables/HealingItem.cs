using UnityEngine;

namespace Curry.Game
{
    public class HealingItem : BaseItem
    {
        [SerializeField] protected float m_healAmount = default;
        protected override bool OnActivate(BaseCharacter hit) 
        {
            hit.OnHeal(m_healAmount);
            return true;
        }
    }
}
