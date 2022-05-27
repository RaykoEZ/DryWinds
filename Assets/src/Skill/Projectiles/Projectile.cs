using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public class Projectile : Interactable, ISkillObject<VectorInput>
    {
        [SerializeField] protected Animator m_anim = default;
        public virtual GameObject Self { get { return gameObject; } }

        public virtual void Begin(VectorInput param)
        {
            throw new System.NotImplementedException();
        }

        public virtual void End()
        {
            OnDefeat();
        }

        protected virtual void OnHit(BodyPart part)
        {
        }
    }
}