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
        Vector2 m_facingAt = Vector2.down;

        public event OnCharacterTakeDamage OnTakingDamage;
        public override BaseNpc Character { get { return m_npc; } }

        public Vector2 FaceDirection { get { return m_facingAt; } set { m_facingAt = value; } }
        
        protected override void Start() 
        {
            base.Start();
            m_npc.OnTakingDamage += OnTakeDamage;
        }

        public override void OnBasicSkill(BaseCharacter target)
        {
            if (IsReady)
            {
                ActionCall = StartCoroutine(UsingSkill(target));
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

        protected override void OnInterrupt()
        {
            base.OnInterrupt();
            m_anim.SetBool("WindingUp", false);
        }
    }
}
