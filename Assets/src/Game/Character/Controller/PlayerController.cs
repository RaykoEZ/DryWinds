using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Curry.Skill;

namespace Curry.Game
{
    public class PlayerController : BaseCharacterController<Player>
    {
        [SerializeField] Player m_player = default;
        [SerializeField] AnimatorHandler m_anim = default;
        [SerializeField] InputActionReference m_movementAction = default;

        public override Player Character { get { return m_player; } } 

        void Update()
        {
            if (!m_disableInput) 
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    Vector2 pos = Character.CurrentCamera.
                        ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    TargetPosition target = new TargetPosition(pos);
                    OnDrawSkill(target);
                }

                if (m_movementAction.action.ReadValue<Vector2>().sqrMagnitude > 0)
                {
                    Move(m_movementAction.action.ReadValue<Vector2>());
                }
            }
        }

        public void OnBasicSkill(InputAction.CallbackContext c)
        {
            if (!m_basicSkill.IsCurrentSkillAvailable || m_disableInput) 
            {
                return;
            }

            if (c.interaction is PressInteraction)
            {
                switch (c.phase)
                {
                    case InputActionPhase.Performed:
                        if (Mouse.current.rightButton.isPressed)
                        {                            
                            // trigger dash windup anim on rmb press
                            m_anim.OnDashWindUp();
                            m_basicSkill.SkillWindup();
                        }
                        else
                        {
                            m_anim.OnDashRelease();
                            Vector2 pos = Character.CurrentCamera.
                                ScreenToWorldPoint(Mouse.current.position.ReadValue());
                            TargetPosition target = new TargetPosition(pos);
                            OnBasicSkill(target);
                        }
                        break;
                    default:
                        break;
                }
            }
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
                        m_drawSkill.InterruptSkill();
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void OnHitStun(float damage)
        {
            base.OnHitStun(damage);
            // Interrupt the input stun and reapply the stun timer
            m_anim.OnTakeDamage();
        }

        public void ChangeTrace(int index)
        {
        }

        public void NextTrace()
        {

        }

        public void PreviousTrace()
        {

        }
        #endregion


    }
}
