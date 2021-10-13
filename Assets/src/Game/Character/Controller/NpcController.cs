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
        [SerializeField] protected Animator m_anim = default;

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
            m_basicSkill.EquipSkill(skill);
        }

        protected virtual IEnumerator UsingSkill(BaseCharacter target)
        {
            m_anim.SetBool("WindingUp", true);
            m_basicSkill.SkillWindup();
            yield return new WaitForSeconds(Character.BasicSkills.CurrentSkill.Properties.MaxWindupTime);
            m_anim.SetBool("WindingUp", false);
            ActionCall = null;
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
            m_anim.SetBool("WindingUp", false);
        }
    }
}
