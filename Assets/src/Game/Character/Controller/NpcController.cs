using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Ai;

namespace Curry.Game
{

    public class NpcController : BaseCharacterController<BaseNpc>
    {
        [SerializeField] BaseNpc m_npc = default;
        protected Coroutine m_retreat;
        protected override BaseNpc Character { get { return m_npc; } }

        protected override void OnEnable()
        {
            base.OnEnable();
            Character.OnKnockout += OnKnockedout;
            Character.OnKnockoutRecover += OnKnockoutRecovery;
        }

        protected override void Activate()
        {

            base.Activate();
        }

        protected override void Deactivate()
        {
            base.Deactivate();
        }

        public override void OnBasicSkill(BaseCharacter target)
        {
            if (IsReady)
            {
                m_basicSkill.ActivateSkill(target.transform.position);
            }
        }

        protected virtual IEnumerator ShowHabit() 
        {
            m_anim.SetBool("ShowHabit", true);
            yield return new WaitUntil(() => !m_anim.GetBool("ShowHabit"));
        }

        protected override void OnHitStun(float stunMod)
        {
            if (m_retreat != null)
            {
                // Interrupt retreat
                InterruptRetreat();
            }
            if (!m_anim.GetBool("KnockedOut"))
            {
                m_anim.SetTrigger("Panic");
            }
            base.OnHitStun(stunMod);
        }

        protected virtual void OnKnockedout() 
        {
            Deactivate();
            m_anim.SetBool("KnockedOut", true);
        }

        protected virtual void OnKnockoutRecovery() 
        {
            m_anim.SetBool("KnockedOut", false);
            StartCoroutine(Recover());
        }

        IEnumerator Recover() 
        {
            yield return new WaitForSeconds(Character.CurrentStats.HitRecoveryTime);
            Activate();
        }

        public virtual void Flee() 
        {
            NpcTerritory target = Character.ChooseRetreatDestination();
            if(target != null) 
            {
            }
        }
        protected virtual void Retreat()
        {
            m_retreat = StartCoroutine(OnRetreatSequence());
        }
        protected virtual void InterruptRetreat()
        {
            StopCoroutine(m_retreat);
            m_retreat = null;          
        }
        public virtual void Wander() 
        {
        }

        public virtual void EquipBasicSkill(ICharacterAction<IActionInput> skill)
        {
            if (m_basicSkill.CurrentSkill != null)
            {
                m_basicSkill.CurrentSkill.OnFinish -= OnActionFinish;
            }
            m_basicSkill.CurrentSkill = skill;
            m_basicSkill.CurrentSkill.OnFinish += OnActionFinish;
        }

        protected override IEnumerator RecoverInput(float stunMod)
        {
            yield return base.RecoverInput(stunMod);

        }

        protected virtual IEnumerator OnRetreatSequence()
        {
            m_anim.SetTrigger("Retreat");
            yield return new WaitUntil(() => m_anim.GetBool("Retreated"));
            Deactivate();
            Character.Retreat();
        }

        protected override void InterruptSkill()
        {
            base.InterruptSkill();
        }
    }
}
