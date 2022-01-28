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
                ActionCall = StartCoroutine(UsingSkill(target));
            }
        }

        public override void MoveTo(Vector2 target, float unitPerStep = 0.1f)
        {
            if (IsReady)
            {
                ActionCall = StartCoroutine(OnMove(target));
            }
        }
        public virtual void MoveTo(Transform target)
        {
            if (IsReady)
            {
                ActionCall = StartCoroutine(OnMove(target));
            }
        }

        public virtual void InterruptAction() 
        {
            if(ActionCall!= null) 
            {
                StopCoroutine(ActionCall);
                ActionCall = null;
            }
        }

        public virtual void Wander() 
        {
            if (IsReady) 
            {
                m_pathHandler.Wander();
            }
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

        protected virtual IEnumerator OnMove(Vector2 target) 
        {
            PathHandler.PlanPath(target);
            yield return new WaitUntil(() => { return PathHandler.TargetReached; });
            Debug.Log("Path reached");
            ActionCall = null;
        }
        protected virtual IEnumerator OnMove(Transform target)
        {
            PathHandler.PlanPath(target);
            yield return new WaitUntil(() => { return PathHandler.TargetReached; });
            Debug.Log("Path reached");
            ActionCall = null;
        }

        protected override void OnInterrupt()
        {
            base.OnInterrupt();
            Character.Animator.SetBool("WindingUp", false);
        }
    }
}
