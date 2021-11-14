using UnityEngine;

namespace Curry.Game
{
    public abstract class ModifierItem : BaseItem 
    {
        [SerializeField] protected float m_duration = default;
        [SerializeField] protected CharacterModifierProperty m_modifierValue = default;
        protected virtual CharactertModifier Modifier { get; set; }

        protected override bool OnActivate(BaseCharacter hit)
        {
            hit.ApplyModifier(Modifier);
            return true;
        }
    }
}
