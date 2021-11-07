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

        Coroutine ActionCall { get; set; }
        public bool IsReady { get { return ActionCall == null; } }
        public event OnCharacterTakeDamage OnTakingDamage;
        public override BaseNpc Character { get { return m_npc; } }
        
        protected override void Start() 
        {
            base.Start();
            m_npc.OnTakingDamage += OnTakeDamage;
        }

        public override void OnBasicSkill(ITargetable<Vector2> target)
        {
            m_anim.SetBool("WindingUp", false);
            m_basicSkill.ActivateSkill(target);
        }

        public virtual void OnSkillWindup(BaseCharacter target)
        {
            ActionCall = StartCoroutine(UseBasicSkill(target));
        }

        protected virtual void OnTakeDamage(float damage)
        {
            OnTakingDamage?.Invoke(damage);
        }
        public virtual void EquipBasicSkill(ICharacterAction<IActionInput, SkillProperty> skill)
        {
            m_basicSkill.EquipSkill(skill);
        }

        protected virtual IEnumerator UseBasicSkill(BaseCharacter target)
        {
            m_anim.SetBool("WindingUp", true);
            m_basicSkill.SkillWindup();
            yield return new WaitForSeconds(Character.BasicSkills.CurrentSkill.Properties.MaxWindupTime);
            TargetPosition pos = new TargetPosition(target.transform.position);
            OnBasicSkill(pos);
            ActionCall = null;
        }

    }
}
