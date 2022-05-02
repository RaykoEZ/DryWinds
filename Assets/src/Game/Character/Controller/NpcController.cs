using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Ai;

namespace Curry.Game
{
    [RequireComponent(typeof(IPathAi))]
    public class NpcController : BaseCharacterController<BaseNpc>
    {
        [SerializeField] BaseNpc m_npc = default;
        protected Coroutine m_retreat;

        public PathState MovementState { get { return PathHandler.State; } }
        protected IPathAi m_pathHandler;
        protected override BaseNpc Character { get { return m_npc; } }
        protected virtual IPathAi PathHandler { get { return m_pathHandler; } }

        protected override void OnEnable()
        {
            base.OnEnable();
            Character.OnKnockout += OnKnockedout;
            Character.OnKnockoutRecover += OnKnockoutRecovery;
        }

        protected override void Activate()
        {
            m_pathHandler = GetComponent<IPathAi>();
            PathHandler.OnReached += OnDestinationReached;
            PathHandler.Startup();
            base.Activate();
        }

        protected override void Deactivate()
        {
            PathHandler.Stop();
            base.Deactivate();
        }

        public override void OnBasicSkill(BaseCharacter target)
        {
            if (IsReady)
            {
                m_actionCall = StartCoroutine(UsingSkill(target));
            }
        }

        protected virtual void OnDestinationReached() 
        {
            switch (PathHandler.State)
            {
                case PathState.Wandering:
                    // Start doing other actions
                    StartCoroutine(ShowHabit());
                    break;
                case PathState.Fleeing:
                    Retreat();
                    break;
                default:
                    break;
            }
        }

        protected virtual IEnumerator ShowHabit() 
        {
            PathHandler.Stop();
            m_anim.SetBool("ShowHabit", true);
            yield return new WaitUntil(() => !m_anim.GetBool("ShowHabit"));
            PathHandler.Startup();
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
            m_anim.SetBool("KnockedOut", true);
            Deactivate();
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
                PathHandler.Flee(target);
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
            m_pathHandler.Wander();
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
            PathHandler.Stop();
            yield return base.RecoverInput(stunMod);
            PathHandler.Startup();
        }

        protected virtual IEnumerator UsingSkill(BaseCharacter target)
        {
            m_anim.SetBool("WindingUp", true);
            yield return new WaitForSeconds(Character.BasicSkills.CurrentSkill.Properties.WindupTime);
            m_anim.SetBool("WindingUp", false);
            m_basicSkill.ActivateSkill(target.transform.position);
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
            m_anim.SetBool("WindingUp", false);
        }
        protected override void InterruptAction()
        {
            base.InterruptAction();
            m_actionCall = null;
        }

    }
}
