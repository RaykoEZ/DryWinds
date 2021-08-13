using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Curry.Game;

namespace Curry.Skill
{
    // NOTE TO SELF: IN THE FUTURE, DASH TRAIL AND HIT BOX LOGICS NEED TO
    // MOVE INTO STATEMACHINE BEHAVIOUR FOR MORE CONTROL DURING SKILL ANIMATION

    public class ChargeAttack : BaseSkill 
    {
        [SerializeField] protected float m_chargeDuration = default;

        public override void SkillWindup() 
        {
            base.SkillWindup();
            m_animator.SetBool("WindingUp", true);
        }

        public override void Activate(SkillTargetParam target = null)
        {
            if(target == null) 
            { 
                return; 
            }

            m_animator.SetTrigger("SkillTrigger");
            base.Activate(target);
        }

        protected override IEnumerator SkillEffect(SkillTargetParam target = null)
        {
            float chargeFactor = Mathf.Max(
            0.5f,
            (Mathf.Min(m_windupTimer, m_maxWindupTime)) / m_maxWindupTime);
            Vector2 mousePos = target.TargetPos;

            m_windupTimer = 0f;
            m_animator.SetBool("WindingUp", false);
            StartCoroutine(DashMotion(m_user.RigidBody.position, mousePos, chargeFactor));

            yield return null;
        }

        protected virtual IEnumerator DashMotion(Vector2 origin, Vector2 targetPos, float chargeCoeff)
        {
            float t = 0f;
            Vector2 dir = targetPos - origin;
            Rigidbody2D rb = m_user.RigidBody;
            while (t < m_chargeDuration)
            {
                rb.AddForce(dir.normalized * chargeCoeff * m_user.CurrentStats.Speed, ForceMode2D.Impulse);
                t += Time.deltaTime;
                yield return null;
            }

            t = 0f;
            float dragSum = 0f;
            while (t < m_chargeDuration)
            {
                rb.drag += 0.5f;
                dragSum += 0.5f;
                t += Time.deltaTime;
                yield return null;
            }
            rb.drag -= dragSum;
        }

        public override void StopIframe() 
        {
            base.StopIframe();
        }
    }
}
