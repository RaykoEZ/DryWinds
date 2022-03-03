using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public class EffectOverTimeModifier : CharacterModifier
    {
        [SerializeField] protected float m_interval = default;
        protected float m_timer = 0f;
        protected Action m_triggerAction = default;
        public override ModifierOpType Type { get { return ModifierOpType.Special;} }

        public EffectOverTimeModifier(
            string name, 
            CharacterModifierProperty value, 
            float duration, 
            float triggerInterval,
            Action trigger):
            base(name, value, duration)
        {
            m_interval = triggerInterval;
            m_triggerAction = trigger;
        }

        public override void OnTimeElapsed(float dt)
        {
            m_timer += dt;
            if(m_timer >= m_interval) 
            {
                TriggerEffect();
            }

            base.OnTimeElapsed(dt);
        }

        public override CharacterModifierProperty Apply(CharacterModifierProperty baseVal)
        {
            return baseVal;
        }

        public override CharacterModifierProperty Revert(CharacterModifierProperty baseVal) 
        {
            return baseVal;
        }

        protected override void TriggerEffect() 
        {
            m_triggerAction?.Invoke();
            m_timer = 0f;
            base.TriggerEffect();
        }
    }
}
