using System.Collections;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public abstract class BaseCharacterController<T> : MonoBehaviour where T : BaseCharacter
    { 
        public abstract T Character { get; }
        protected bool m_disableInput = false;
        protected Coroutine m_currentInputRecovery;
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
            float drag = Character.RigidBody.drag;
            Character.RigidBody.AddForce(dir * Character.CurrentStats.Speed * drag);
        }

        public virtual void OnDrawSkill(ITargetable<Vector2> target) 
        {
            m_drawSkill.ActivateSkill(target);
        }
        public virtual void OnBasicSkill(ITargetable<Vector2> target) 
        {
            m_basicSkill.ActivateSkill(target);
        }

        protected virtual void OnHitStun(float damage)
        {
            // Interrupt the input stun and reapply the stun timer
            if (m_currentInputRecovery != null)
            {
                StopCoroutine(m_currentInputRecovery);
            }
            OnInterrupt();
            m_currentInputRecovery = StartCoroutine(RecoverInput());
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
            m_disableInput = true;
            yield return new WaitForSeconds(Character.CurrentStats.HitRecoveryTime);
            m_disableInput = false;
            m_currentInputRecovery = null;
        }
    }
}
