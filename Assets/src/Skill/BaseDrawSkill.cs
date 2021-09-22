using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class BaseDrawSkill : BaseSkill
    {
        [SerializeField] protected PrefabAsset m_trace = default;
        [SerializeField] protected InteractableInstanceManager m_instanceManager = default;

        protected Vector2 m_previousDrawPos;
        protected BaseTrace m_currentTracerBehaviour;
        public virtual float CooldownTime { get { return m_skillProperty.CooldownTime; } }

        public override void Init(BaseCharacter user, bool hitBoxOn = false)
        {
            base.Init(user, hitBoxOn);
            m_instanceManager.PrepareNewInstance(m_trace);
        }
        protected override IEnumerator SkillEffect(SkillParam target = null) 
        {
            yield break;
        }

        public override void Execute(SkillParam param)
        {
            // If we cannot use skill, end here
            if (!SkillUsable)
            {
                EndSkillEffect();
                return;
            }

            if (param is VectorParam posParam) 
            {
                // start a new stroke if we hold LMB (already drawing) and is moving
                if (!m_skillInProgress)
                {
                    // make new stroke
                    m_currentTracerBehaviour = m_instanceManager.GetInstanceFromCurrentPool() as BaseTrace;
                }
                float length = !m_skillInProgress ? 0f : Vector2.Distance(posParam.Target, m_previousDrawPos);
                float totalCost = length * SkillProperties.SpCost;
                //update mousePos log
                m_previousDrawPos = posParam.Target;
                ConsumeResource(totalCost);
                m_currentTracerBehaviour.Execute(posParam.Target, length);
                m_skillInProgress = true;
            }

        }

        public override void EndSkillEffect()
        {
            base.EndSkillEffect();
            CoolDown();
        }
    }
}
