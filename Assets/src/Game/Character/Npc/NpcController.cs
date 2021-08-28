using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class NpcController : MonoBehaviour
    {
        [SerializeField] protected BaseNpc m_npc = default;
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] protected SkillHandler m_skillHandler = default;
        [SerializeField] float m_actionInterval = default;
        protected float m_actionTimer = 0f;
        protected bool m_attacking = false;
        protected virtual void Start() 
        {
            m_skillHandler.Init(m_npc);
            m_npc.OnTakingDamage += OnTakeDamage;
            m_npc.OnDefeated += OnDefeat;
        }

        protected virtual void Update() 
        {
            if (m_actionInterval != 0f && !m_attacking && m_actionTimer > m_actionInterval) 
            {
                m_attacking = true;
                m_actionTimer = 0f;
                OnSkill();
            }
            else 
            {
                m_actionTimer += Time.deltaTime;
            }
        }

        protected virtual void OnSkill()
        {
            StartCoroutine(WindupSkill());
        }

        protected virtual IEnumerator WindupSkill() 
        {
            m_anim.SetBool("WindingUp", true);
            m_skillHandler.SkillWindup();
            yield return new WaitForSeconds(m_skillHandler.CurrentSkill.MaxWindUpTime);
            m_anim.SetBool("WindingUp", false);
            m_skillHandler.ActivateSkill(m_npc.Target.position);
            m_attacking = false;
        }

        protected virtual void OnTakeDamage(float damage) 
        { 
        
        }

        protected virtual void OnDefeat() 
        { 

        }
    }
}
