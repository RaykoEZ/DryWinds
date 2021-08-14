﻿using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class ChargeAttack : BaseSkill, IHitboxEffect 
    {
        [SerializeField] protected float m_chargeDuration = default;

        public override void SkillWindup() 
        {
            base.SkillWindup();
            m_animator.SetBool("WindingUp", true);
        }

        protected virtual void OnTriggerEnter2D(Collider2D col) 
        {
            BaseCharacter hit = col.gameObject.GetComponent<BaseCharacter>();
            OnHit(hit);
        }

        public void OnHit(BaseCharacter hit)
        {
            if (hit == null || (hit.Relations & m_skillProperty.TargetOptions) == ObjectRelations.None)
            {
                return;
            }

            Vector2 diff = hit.RigidBody.position - m_user.RigidBody.position;
            hit.OnKnockback(diff.normalized, m_skillProperty.Knockback);
            hit.OnTakeDamage(m_skillProperty.StaminaDamage);
        }

        public override void Execute(SkillTargetParam target = null)
        {
            if(target == null) 
            { 
                return; 
            }

            m_animator.SetTrigger("SkillTrigger");
            base.Execute(target);
        }

        protected override IEnumerator SkillEffect(SkillTargetParam target = null)
        {
            float chargeFactor = Mathf.Max(
            0.4f,
            (Mathf.Min(m_windupTimer, m_skillProperty.MaxWindupTime)) / m_skillProperty.MaxWindupTime);
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