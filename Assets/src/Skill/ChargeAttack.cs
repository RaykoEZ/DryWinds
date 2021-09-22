using System;
using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class ChargeAttack : BaseSkill, IHitboxEffect 
    {
        [SerializeField] protected float m_chargeDuration = default;
        Coroutine m_dashing;
        public override void SkillWindup() 
        {
            base.SkillWindup();
            m_animator.SetBool("WindingUp", true);
        }

        public override void OnHit(Interactable hit)
        {
            Vector2 diff = hit.RigidBody.position - m_user.RigidBody.position;
            hit.OnKnockback(diff.normalized, m_skillProperty.Knockback);
            hit.OnTakeDamage(m_skillProperty.StaminaDamage);
        }

        public override void Execute(SkillParam target)
        {
            if(target == null) 
            { 
                return; 
            }

            m_animator.SetTrigger("SkillTrigger");
            base.Execute(target);
        }

        protected override IEnumerator SkillEffect(SkillParam target)
        {
            if(target is VectorParam posParam) 
            {
                float chargeFactor = Mathf.Max(
                    0.4f,
                    (Mathf.Min(m_windupTimer, m_skillProperty.MaxWindupTime)) / m_skillProperty.MaxWindupTime);
                Vector2 mousePos = posParam.Target;

                m_windupTimer = 0f;
                m_animator.SetBool("WindingUp", false);
                m_dashing = StartCoroutine(DashMotion(m_user.RigidBody.position, mousePos, chargeFactor));
            }

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
            float oldDrag = rb.drag;
            while (t < m_chargeDuration)
            {
                rb.drag += 0.5f;
                t += Time.deltaTime;
                yield return null;
            }

            rb.drag = oldDrag;
            OnSkillFinish();
        }

        public override void StopIframe() 
        {
            base.StopIframe();
        }

        public override void Interrupt()
        {
            if(m_dashing != null) 
            {
                StopCoroutine(m_dashing);
            }
            base.Interrupt();
        }
    }
}
