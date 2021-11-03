using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Ai;

namespace Curry.Game
{
    [RequireComponent(typeof(IPathHandler))]
    public class NpcController : BaseCharacterController<BaseNpc>
    {
        [SerializeField] protected BaseNpc m_npc = default;

        protected IPathHandler m_pathHandler;
        public event OnCharacterTakeDamage OnTakingDamage;
        public override BaseNpc Character { get { return m_npc; } }
        protected virtual IPathHandler PathHandler { get { return m_pathHandler; } }

        protected void Awake()
        {
            m_pathHandler = GetComponent<IPathHandler>();
        }

        protected override void OnEnable() 
        {
            base.OnEnable();
            m_npc.OnTakingDamage += OnTakeDamage;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            m_npc.OnTakingDamage -= OnTakeDamage;
        }

        public override void OnBasicSkill(BaseCharacter target)
        {
            if (IsReady)
            {
                ActionCall = StartCoroutine(UsingSkill(target));
            }
        }

        public override void Move(Vector2 target)
        {
            if (IsReady)
            {
                PathHandler.OnPlanned += OnPathPlanned;
                PathHandler.PlanPath(target);
                ActionCall = StartCoroutine(OnMove());
            }
        }
        public virtual void Move(Transform target)
        {
            if (IsReady)
            {
                PathHandler.OnPlanned += OnPathPlanned;
                PathHandler.PlanPath(target);
                ActionCall = StartCoroutine(OnMove());
            }
        }

        protected virtual void OnPathPlanned(bool pathPossible) 
        {
            if (pathPossible) 
            {
                PathHandler.OnPlanned -= OnPathPlanned;
                PathHandler.FollowPlannedPath();
            }
        }

        protected virtual void OnTakeDamage(float damage)
        {
            OnTakingDamage?.Invoke(damage);
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

        protected virtual IEnumerator OnMove() 
        {
            yield return new WaitUntil(() => { return PathHandler.TargetReached; });
            ActionCall = null;
        }

        protected override void OnInterrupt()
        {
            base.OnInterrupt();
            Character.Animator.SetBool("WindingUp", false);
        }
    }
}
