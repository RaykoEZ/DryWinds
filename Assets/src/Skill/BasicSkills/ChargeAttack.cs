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
        public override void Windup() 
        {
            base.Windup();
            m_animator.SetBool("WindingUp", true);
        }

        public override void OnHit(Interactable hit)
        {
            if (m_dashing != null) 
            {
                StopCoroutine(m_dashing);
                OnSkillFinish();
            }

            Vector2 diff = m_user.RigidBody.position - hit.RigidBody.position;
            if (hit.RigidBody.bodyType != RigidbodyType2D.Static)
            {
                hit.RigidBody.velocity = Vector2.zero;
                hit.OnKnockback(-diff.normalized, m_skillProperty.Knockback);
            }

            m_user.RigidBody.velocity = Vector2.zero;
            m_user.OnKnockback(diff.normalized, 0.75f * m_skillProperty.Knockback);
            hit.OnTakeDamage(m_skillProperty.ActionValue);
        }

        public override void Execute(IActionInput target)
        {
            if(target == null) 
            { 
                return; 
            }

            m_animator.SetTrigger("SkillTrigger");
            base.Execute(target);
        }

        protected override IEnumerator SkillEffect(IActionInput target)
        {
            if(target is VectorInput posParam) 
            {
                float chargeFactor = Mathf.Max(
                    0.4f,
                    (Mathf.Min(m_windupTimer, m_skillProperty.MaxWindupTime)) / m_skillProperty.MaxWindupTime);
                Vector2 mousePos = posParam.Target;
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
            yield return null;
            OnSkillFinish();
        }
    }
}
