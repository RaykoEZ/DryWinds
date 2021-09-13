using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{

    public class DrawSkillBrush : SkillHandler
    {
        [SerializeField] protected float m_cooldownTime = default;

        protected bool m_isDrawing = false;
        protected bool m_onCD = false;
        protected Vector2 m_previousDrawPos;
        protected BaseCharacter m_user;
        protected BaseTrace m_currentTracerBehaviour;

        public virtual float CooldownTime { get { return m_cooldownTime; } }

        public virtual void Init(BaseCharacter user, Asset initSkill) 
        {
            m_user = user;
            PrepareSkill(initSkill);
        }

        public override void ActivateSkill(Vector2 targetPos, Dictionary<string, object> payload = null)
        {
            // don't draw when we are on cooldown
            if (m_onCD)
            {
                return;
            }
            
            // start a new stroke if we hold LMB and is moving
            if (!m_isDrawing)
            {
                // make new stroke
                CurrentSkill = GetNewSkillInstance().GetComponent<BaseTrace>();
                CurrentSkill.Init(m_user, true);
            }

            // If we cannot use skill, end here
            if (!CurrentSkill.SkillUsable)
            {
                OnDrawEnd();
                return;
            }

            float length = !m_isDrawing ? 0f : Vector2.Distance(targetPos, m_previousDrawPos);
            //update mousePos log
            m_previousDrawPos = targetPos;
            SkillParam param =
                new SkillParam(
                    targetPos,
                    new Dictionary<string, object> { { "length", length} });

            m_isDrawing = true;
            ExecuteSkill(param);
        }

        public virtual void OnDrawEnd()
        {
            m_isDrawing = false;
            StartCoroutine(OnCoolDown());
        }

        protected virtual IEnumerator OnCoolDown() 
        {
            m_onCD = true;
            //start cooldown and reset skill states
            yield return new WaitForSeconds(m_cooldownTime);
            m_onCD = false;
        }

    }
}
