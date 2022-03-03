using System;
using System.Collections;
using UnityEngine;
using Curry.Game;
using Curry.Ai;

namespace Curry.Skill
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ChargeAttack : BaseSkill 
    {
        [SerializeField] protected float m_chargeDuration = default;
        [SerializeField] protected float m_recoilTime = default;

        protected override void OnHit(BodyPart part)
        {
            if (m_execute != null) 
            {
                Interrupt();
            }
            
            if (part != null) 
            {
                Vector2 source = m_user.RigidBody.position;
                part.Hit(m_skillProperty.ActionValue,
                    m_skillProperty.Knockback,
                    source);
            }

            Vector2 v = m_user.RigidBody.velocity.normalized;
            m_user.RigidBody.velocity *= 0.1f;
            m_user.OnKnockback(-v, 0.5f * m_skillProperty.Knockback);
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
