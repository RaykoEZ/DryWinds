using System.Collections;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public abstract class BaseCharacterController<T> : MonoBehaviour where T : BaseCharacter
    {
        public virtual bool IsReady { get { return m_actionCall == null; } }
        protected Coroutine m_actionCall = null;
        protected abstract T Character { get; }

        protected SkillActivator m_basicSkill = new SkillActivator();
        protected SkillActivator m_drawSkill = new SkillActivator();

        protected virtual void OnEnable()
        {
            Character.OnHitStun += OnHitStun;
            Character.OnActionInterrupt += InterruptSkill;
            Character.OnLoaded += Init;
            Character.OnDefeated += OnDefeated;
        }

        protected virtual void OnDisable() 
        {
            Character.OnHitStun -= OnHitStun;
            Character.OnActionInterrupt -= InterruptSkill;
            Character.OnLoaded -= Init;
            Character.OnDefeated -= OnDefeated;
        }

        protected virtual void Init()
        {
            EquipeSkill(0);
            EquipeDrawSkill(0);
        }

        public virtual void EquipeSkill(int index) 
        {
            Character.BasicSkills.EquippedIndex = index;
            if (m_basicSkill.CurrentSkill != null) 
            {
                m_basicSkill.CurrentSkill.OnFinish -= OnActionFinish;
            }
            m_basicSkill.CurrentSkill = Character.BasicSkills.CurrentSkill;
            m_basicSkill.CurrentSkill.OnFinish += OnActionFinish;
        }
        public virtual void EquipeDrawSkill(int index)
        {
            Character.DrawSkills.EquippedIndex = index;
            if (m_drawSkill.CurrentSkill != null)
            {
                m_drawSkill.CurrentSkill.OnFinish -= OnActionFinish;
            }
            m_drawSkill.CurrentSkill = Character.DrawSkills.CurrentSkill;
            m_drawSkill.CurrentSkill.OnFinish += OnActionFinish;

        }

        protected virtual void OnActionFinish(ICharacterAction<IActionInput> action) 
        {
            m_actionCall = null;
        }

        public virtual void MoveTo(Vector2 direction, float unitPerStep = 0.1f)
        {
            if (IsReady)
            {
                Character.RigidBody.MovePosition(Character.RigidBody.position + (unitPerStep * direction * Character.CurrentStats.Speed));
            }
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
                m_actionCall = StartCoroutine(OnSkill(target));
            }
        }

        public virtual void OnBasicSkill(BaseCharacter target)
        {
            if (IsReady)
            {
                m_actionCall = StartCoroutine(OnSkill(target.transform.position));
            }
        }

        protected virtual IEnumerator OnSkill(Vector2 target) 
        {
            yield return new WaitForSeconds(m_basicSkill.CurrentSkill.Properties.WindupTime);
            m_basicSkill.ActivateSkill(target);
        }

        protected void OnHitStun(float stunMod)
        {
            // Interrupt the input stun and reapply the stun timer
            InterruptAction();
            InterruptSkill();
            m_actionCall = StartCoroutine(RecoverInput(stunMod));
        }
        protected virtual void InterruptSkill()
        {
            m_basicSkill.InterruptSkill();
            m_drawSkill.InterruptSkill();
        }

        protected virtual void OnDefeated() 
        {       
        }

        protected IEnumerator RecoverInput(float stunMod)
        {
            yield return new WaitForSeconds(stunMod * Character.CurrentStats.HitRecoveryTime);
            Character.RigidBody.velocity = Vector2.zero;
            m_actionCall = null;
        }

        protected virtual void InterruptAction()
        {
            if (m_actionCall != null)
            {
                StopCoroutine(m_actionCall);
                m_actionCall = null;
            }
        }
    }
}
