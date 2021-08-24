using UnityEngine;

namespace Curry.Game
{
    public abstract class ModifierItem : BaseItem 
    {
        [SerializeField] protected float m_duration = default;
        [SerializeField] protected CharacterContext m_modifierValue = default;
        [SerializeField] bool m_isBaseModifier = default;
        protected virtual ContextModifier<CharacterContext> Modifier 
        { get; set; }

        public override bool ActivateEffect(BaseCharacter hit)
        {
            hit.ApplyModifier(Modifier, m_isBaseModifier);
            return true;
        }
    }


    public class StatAdderItem: ModifierItem
    {
        public virtual void Start() 
        {
            Modifier = new CharacterAdder(m_itemName, m_modifierValue, m_duration);
        }
    }


    public abstract class EffectOverTimeItem : ModifierItem 
    {
        [SerializeField] protected float m_triggerInterval = default;

        public virtual void Start()
        {
            Modifier = new EffectOverTimeModifier(
                m_itemName, 
                m_modifierValue, 
                m_duration, 
                m_triggerInterval, 
                OnEffectTrigger);
        }

        protected abstract void OnEffectTrigger(CharacterContext current);
    }
}
