using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Curry.Skill;

namespace Curry.Game
{
    public class SkillHandler : MonoBehaviour
    {
        [SerializeField] BaseSkill m_skill = default;

        public bool IsSkillAvailable
        {
            get
            {
                return m_skill.SkillUsable;
            }
        }

        public void Init(BaseCharacter user)
        {
            m_skill.Init(user);
        }

        public virtual void DashWindup()
        {
            m_skill.SkillWindup();
        }

        public virtual void Dash(Vector2 targetPos)
        {
            SkillTargetParam param = new SkillTargetParam(targetPos);
            m_skill.Activate(param);
        }
    }
}
