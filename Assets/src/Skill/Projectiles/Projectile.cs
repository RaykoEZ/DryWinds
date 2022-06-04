using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public class Projectile : Interactable, ISkillObject<VectorInput>
    {
        [SerializeField] protected Animator m_onActivate = default;
        public virtual GameObject Self { get { return gameObject; } }
        protected ProjectileSetting m_currentSetting;
        protected float m_currentDamage;
        protected float m_currentknockBack;
        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            BodyPart part = col.gameObject.GetComponent<BodyPart>();
            if (part != null)
            {
                OnHit(part);
            }
        }

        public virtual void Begin(VectorInput param)
        {
            if (param.Payload == null) { return; }

            if (param.Payload["Setting"] is ProjectileSetting setting && 
                param.Payload["Damage"] is float damage &&
                param.Payload["Knockback"] is float knockback) 
            {
                m_currentSetting = setting;
                m_currentDamage = damage;
                m_currentknockBack = knockback;
                m_onActivate.SetTrigger("Start");
                RigidBody.AddForce(setting.InitForce * param.Value.normalized);
            }
        }

        public virtual void End()
        {
            m_onActivate.SetTrigger("End");
            OnDefeat();
        }

        protected virtual void OnHit(BodyPart part)
        {
            if (part != null)
            {
                part.Hit(
                    m_currentDamage,
                    m_currentknockBack,
                    transform.position);
            }
            End();
        }
    }
}