using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Curry.Skill;

namespace Curry.Game
{
    public class DrawSkillManager : MonoBehaviour
    {
        [SerializeField] Camera m_cam = default;
        [SerializeField] BaseCharacter m_user = default;
        protected SkillActivator m_skillActivator = new SkillActivator();

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                Vector2 pos = m_cam.
                    ScreenToWorldPoint(Mouse.current.position.ReadValue());
                UseDrawSkill(pos);
            }
            else
            {
                m_user.OnSPRegen();
            }
        }

        public void EquipSkill(string name) 
        {
            foreach (BaseSkill skill in m_user.DrawSkills.Skills)
            {
                if (skill.Properties.Name == name)
                {
                    m_skillActivator.CurrentSkill = skill;
                }
            }
        }

        public virtual void UseDrawSkill(Vector2 target)
        {
            if(m_skillActivator.CurrentSkill == null) 
            {
                m_skillActivator.CurrentSkill = m_user.DrawSkills.Skills[0];
            }
            m_skillActivator.ActivateSkill(target);
        }
        #region drawing brush
        public void OnDrawSkill(InputAction.CallbackContext c)
        {
            if (c.interaction is PressInteraction)
            {
                switch (c.phase)
                {
                    case InputActionPhase.Performed:
                        // Finished a brush stroke
                        m_skillActivator.InterruptSkill();
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}