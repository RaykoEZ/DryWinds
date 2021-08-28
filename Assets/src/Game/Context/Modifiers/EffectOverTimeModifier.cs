using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public class EffectOverTimeModifier : CharactertModifier
    {
        [SerializeField] protected float m_interval = default;
        protected float m_timer = 0f;
        protected Action<CharacterContext> m_triggerAction = default;
        public override ModifierOpType Type { get { return ModifierOpType.Special;} }

        public EffectOverTimeModifier(
            string name, 
            CharacterModifierProperty value, 
            float duration, 
            float triggerInterval,
            Action<CharacterContext> trigger):
            base(name, value, duration)
        {
            m_interval = triggerInterval;
            m_triggerAction = trigger;
        }

        public override void OnTimeElapsed(float dt, CharacterContext current)
        {
            m_timer += dt;
            if(m_timer >= m_interval) 
            {
                TriggerEffect(current);
            }

            base.OnTimeElapsed(dt, current);
        }

        public override CharacterModifierProperty Apply(CharacterModifierProperty baseVal)
        {
            return baseVal;
        }

        public override CharacterModifierProperty Revert(CharacterModifierProperty baseVal) 
        {
            return baseVal;
        }

        protected override void TriggerEffect(CharacterContext current) 
        {
            m_triggerAction?.Invoke(current);
            m_timer = 0f;
            base.TriggerEffect(current);
        }
    }
}
