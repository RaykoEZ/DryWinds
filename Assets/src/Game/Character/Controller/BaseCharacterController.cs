using System.Collections;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public abstract class BaseCharacterController<T> : MonoBehaviour where T : BaseCharacter
    {
        public virtual bool IsReady { get { return ActionCall == null; } }
        protected Coroutine ActionCall { get; set; }
        public abstract T Character { get; }

        protected SkillActivator m_basicSkill = new SkillActivator();
        protected SkillActivator m_drawSkill = new SkillActivator();

        protected virtual void Start()
        {
            Character.OnTakingDamage += OnHitStun;
            Character.OnActionInterrupt += OnInterrupt;
            Character.OnLoaded += Init;
            Character.OnDefeated += OnDefeated;
        }
        protected virtual void Init()
        {
            m_basicSkill.EquipSkill(Character.BasicSkills.CurrentSkill);
            m_drawSkill.EquipSkill(Character.DrawSkills.CurrentSkill);
        }
        public virtual void Move(Vector2 dir)
        {
            if (IsReady) 
            {
                float drag = Character.RigidBody.drag;
                Character.RigidBody.AddForce(dir * Character.CurrentStats.Speed * drag);
            }
        }

        public virtual void MoveTo(Vector2 pos)
        {
            if (IsReady) 
            {
                ActionCall = StartCoroutine(Movement(pos));
            }
        }

        protected virtual IEnumerator Movement(Vector2 pos)
        {
            float t = 0f;
            float distance = Vector2.Distance(pos, transform.position);
            float duration = distance / Character.CurrentStats.Speed;
            while (t < duration) 
            {
                t += Time.deltaTime;
                Character.transform.position = Vector2.Lerp(transform.position, pos, t/duration);
                yield return null;
            }
            ActionCall = null;
        }

        public virtual void OnDrawSkill(Vector2 target) 
        {
            if (IsReady)
            {
                m_drawSkill.ActivateSkill(target);
            }
        }
        public virtual void OnBasicSkill(Vector2 target) 
        {
            if (IsReady)
            {
                m_basicSkill.ActivateSkill(target);
            }
        }

        public virtual void OnBasicSkill(BaseCharacter target)
        {
            if (IsReady)
            {
                m_basicSkill.ActivateSkill(target.transform.position);
            }
        }

        protected void OnHitStun(float damage)
        {
            // Interrupt the input stun and reapply the stun timer
            if (ActionCall != null)
            {
                StopCoroutine(ActionCall);
                ActionCall = null;
            }
            OnInterrupt();
            ActionCall = StartCoroutine(RecoverInput());
        }
        protected virtual void OnInterrupt()
        {
            m_basicSkill.InterruptSkill();
            m_drawSkill.InterruptSkill();
        }

        protected virtual void OnDefeated() 
        {       
        }

        protected IEnumerator RecoverInput()
        {
            yield return new WaitForSeconds(Character.CurrentStats.HitRecoveryTime);
            ActionCall = null;
        }
    }
}
