using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public class NpcController : BaseCharacterController<BaseNpc>
    {
        [SerializeField] protected BaseNpc m_npc = default;
        [SerializeField] protected Animator m_anim = default;

        protected Coroutine m_skillCall;
        public event OnCharacterTakeDamage OnTakingDamage;
        public override BaseNpc Character { get { return m_npc; } }

        protected override void Start() 
        {
            base.Start();
            m_npc.OnTakingDamage += OnTakeDamage;
        }

        public override void OnBasicSkill(ITargetable<Vector2> target)
        {
            // Do not overlap skill calls
            if (m_skillCall != null)
            {
                return;
            }
            m_skillCall = StartCoroutine(UseSkill(target));
        }

        protected virtual void OnTakeDamage(float damage)
        {
            OnTakingDamage?.Invoke(damage);
        }

        protected virtual IEnumerator UseSkill(ITargetable<Vector2> target) 
        {
            m_anim.SetBool("WindingUp", true);
            m_basicSkill.SkillWindup();
            yield return new WaitForSeconds(Character.BasicSkills.CurrentSkill.SkillProperties.MaxWindupTime);
            m_anim.SetBool("WindingUp", false);
            m_basicSkill.ActivateSkill(target);
            m_skillCall = null;
        }
    }
}
