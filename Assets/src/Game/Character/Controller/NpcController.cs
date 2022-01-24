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
        [SerializeField] protected BaseNpc m_npc = default;
        protected IPathAi m_pathHandler;
        public event OnNpcEvaluate OnEvaluate;
        public override BaseNpc Character { get { return m_npc; } }
        protected virtual IPathAi PathHandler { get { return m_pathHandler; } }

        protected void Awake()
        {
            m_pathHandler = GetComponent<IPathAi>();
        }

        protected override void OnEnable() 
        {
            base.OnEnable();
            m_npc.OnEvaluate += Evaluate;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            m_npc.OnEvaluate -= Evaluate;
        }

        public override void OnBasicSkill(BaseCharacter target)
        {
            if (IsReady)
            {
                ActionCall = StartCoroutine(UsingSkill(target));
            }
        }

        public override void Move(Vector2 target, float unitPerStep = 0.1f)
        {
            if (IsReady)
            {
                ActionCall = StartCoroutine(OnMove(target));
            }
        }
        public virtual void Move(Transform target)
        {
            if (IsReady)
            {
                ActionCall = StartCoroutine(OnMove(target));
            }
        }

        public virtual void Wander() 
        {
            if (IsReady) 
            {
                m_pathHandler.Wander();
            }
        }

        protected virtual void Evaluate()
        {
            OnEvaluate?.Invoke();
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
