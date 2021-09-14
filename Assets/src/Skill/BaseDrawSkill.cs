using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class BaseDrawSkill : BaseSkill
    {
        [SerializeField] protected Asset m_trace = default;
        [SerializeField] protected InteractableInstanceManager m_instanceManager = default;
        protected bool m_isDrawing = false;
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
                OnDrawEnd();
                return;
            }
            
            // start a new stroke if we hold LMB and is moving
            if (!m_isDrawing)
            {
                // make new stroke
                m_currentTracerBehaviour = m_instanceManager.GetInstanceFromCurrentPool() as BaseTrace;
            }
            float length = !m_isDrawing ? 0f : Vector2.Distance(param.TargetPos, m_previousDrawPos);
            float totalCost = length * SkillProperties.SpCost;
            //update mousePos log
            m_previousDrawPos = param.TargetPos;
            ConsumeResource(totalCost);
            m_currentTracerBehaviour.Execute(param.TargetPos, length);
            m_isDrawing = true;
        }

        public virtual void OnDrawEnd()
        {
            m_isDrawing = false;
            CoolDown();
        }
    }
}
