using System;
using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ChargeAttack : BaseSkill, IHitboxEffect 
    {
        [SerializeField] protected float m_chargeDuration = default;
        [SerializeField] protected float m_recoilTime = default;

        public override void OnHit(Interactable hit)
        {
            if (m_execute != null) 
            {
                Interrupt();
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

        protected override IEnumerator ExecuteInternal(IActionInput target)
        {
            if(target != null && target is VectorInput posParam) 
            {
                Vector2 mousePos = posParam.Target;
                m_execute = StartCoroutine(DashMotion(m_user.RigidBody.position, mousePos));
            }
            yield return null;
        }

        protected virtual IEnumerator DashMotion(Vector2 origin, Vector2 targetPos)
        {
            float t = 0f;
            Vector2 dir = targetPos - origin;
            Rigidbody2D rb = m_user.RigidBody;
            while (t < m_chargeDuration)
            {
                rb.MovePosition(rb.position + (dir.normalized * (t / m_chargeDuration)));
                t += Time.deltaTime;
                yield return null;
            }
            rb.AddForce(dir, ForceMode2D.Impulse);
            yield return new WaitForSeconds(m_recoilTime);
            rb.velocity = Vector2.zero;
            OnSkillFinish();
        }
    }
}
