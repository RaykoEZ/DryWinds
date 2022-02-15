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
        public PathState MovementState { get { return PathHandler.State; } }
        protected IPathAi m_pathHandler;
        protected override BaseNpc Character { get { return m_npc; } }
        protected virtual IPathAi PathHandler { get { return m_pathHandler; } }

        protected void Awake()
        {
            m_pathHandler = GetComponent<IPathAi>();
        }

        public override void OnBasicSkill(BaseCharacter target)
        {
            if (IsReady)
            {
                m_actionCall = StartCoroutine(UsingSkill(target));
            }
        }

        public virtual void Flee() 
        {
            NpcTerritory target = Character.ChooseRetreatDestination();
            PathHandler.Flee(target);
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

        protected virtual IEnumerator UsingSkill(BaseCharacter target)
        {
            Character.Animator.SetBool("WindingUp", true);
            yield return new WaitForSeconds(Character.BasicSkills.CurrentSkill.Properties.WindupTime);
            Character.Animator.SetBool("WindingUp", false);
            m_basicSkill.ActivateSkill(target.transform.position);
        }

        protected override void Deactivate()
        {
            PathHandler.Stop();
            base.Deactivate();
        }

        protected override void Reactivate()
        {
            base.Reactivate();
            PathHandler.Startup();
        }

        protected override void InterruptSkill()
        {
            base.InterruptSkill();
            Character.Animator.SetBool("WindingUp", false);
        }
        protected override void InterruptAction()
        {
            base.InterruptAction();
            m_actionCall = null;
        }

    }
}
