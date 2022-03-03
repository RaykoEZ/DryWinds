using UnityEngine;

namespace Curry.Game
{
    public abstract class EffectOverTimeItem : ModifierItem 
    {
        [SerializeField] protected float m_triggerInterval = default;

        public virtual void Start()
        {
            Modifier = new EffectOverTimeModifier(
                m_itemProperty.Name, 
                m_modifierValue, 
                m_duration, 
                m_triggerInterval, 
                OnEffectTrigger);
        }

        protected abstract void OnEffectTrigger();
    }
}
