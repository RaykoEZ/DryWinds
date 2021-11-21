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

        Coroutine m_dashing;

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
            Debug.Log("HIT");
            m_user.RigidBody.velocity = Vector2.zero;
            m_user.OnKnockback(diff.normalized, 0.75f * m_skillProperty.Knockback);
            hit.OnTakeDamage(m_skillProperty.ActionValue);
        }

        protected override void OnSkillFinish() 
        {
            base.OnSkillFinish();
            m_dashing = null;
        }

        protected override IEnumerator SkillEffect(IActionInput target)
        {
            if(target != null && target is VectorInput posParam) 
            {
                Vector2 mousePos = posParam.Target;
                m_dashing = StartCoroutine(DashMotion(m_user.RigidBody.position, mousePos));
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
                rb.AddForce(dir.normalized * m_user.CurrentStats.Speed, ForceMode2D.Impulse);
                t += Time.deltaTime;
                yield return null;
            }
            yield return null;
            OnSkillFinish();
        }
    }
}
