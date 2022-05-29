using System;
using System.Collections;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public abstract class BaseCharacterController<T> : MonoBehaviour where T : BaseCharacter
    {
        [SerializeField] protected Animator m_anim = default;
        public virtual bool IsReady { get { return m_isReady; } }
        protected bool m_isReady = true;
        protected abstract T Character { get; }

        protected SkillActivator m_basicSkill = new SkillActivator();
        protected SkillActivator m_drawSkill = new SkillActivator();

        protected virtual void OnEnable()
        {
            Activate();
            Character.OnHitStun += OnHitStun;
            Character.OnActionInterrupt += InterruptSkill;
            Character.OnLoaded += OnAssetLoaded;
            Character.OnDefeated += OnDefeat;
        }

        protected virtual void OnDisable() 
        {
            Deactivate();
            Character.OnHitStun -= OnHitStun;
            Character.OnActionInterrupt -= InterruptSkill;
            Character.OnLoaded -= OnAssetLoaded;
            Character.OnDefeated -= OnDefeat;
        }

        protected virtual void OnAssetLoaded()
        {
            EquipeSkill(0);
            EquipeDrawSkill(0);
        }
        protected virtual void Activate()
        {
            m_isReady = true;
        }
        protected virtual void Deactivate()
        {
            m_isReady = false;
            InterruptSkill();
            StopAllCoroutines();
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
            Activate();
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

        protected virtual void OnDefeat(Action onFinish) 
        {
            StartCoroutine(OnDefeatSequence(onFinish));
        }

        protected virtual IEnumerator OnSkill(Vector2 target) 
        {
            yield return new WaitForSeconds(m_basicSkill.CurrentSkill.Properties.WindupTime);
            m_basicSkill.ActivateSkill(target);
        }
        protected virtual IEnumerator OnDefeatSequence(Action onFinish)
        {
            m_anim.SetBool("Defeated", true);
            yield return new WaitUntil(() => { return m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f; });
            Deactivate();
            onFinish();
        }
        protected virtual void OnHitStun(float stunMod)
        {
            // Interrupt the input stun and reapply the stun timer
            Deactivate();
            StartCoroutine(RecoverInput(stunMod));
        }
        protected virtual void InterruptSkill()
        {
            m_basicSkill.InterruptSkill();
            m_drawSkill.InterruptSkill();
        }


        protected virtual IEnumerator RecoverInput(float stunMod)
        {
            yield return new WaitForSeconds(stunMod * Character.CurrentStats.HitRecoveryTime);
            m_isReady = true;
            Character.RigidBody.velocity = Vector2.zero;
        }
    }
}
