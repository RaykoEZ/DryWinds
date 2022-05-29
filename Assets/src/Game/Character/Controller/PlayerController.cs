using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Curry.Collection;

namespace Curry.Game
{
    public class PlayerController : BaseCharacterController<Player>
    {
        [SerializeField] Player m_player = default;
        [SerializeField] InputActionReference m_movementAction = default;

        protected override Player Character { get { return m_player; } }

        void FixedUpdate()
        {
            if (IsReady) 
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    Vector2 pos = Character.CurrentCamera.
                        ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    OnDrawSkill(pos);
                }
                else
                {
                    Character.OnSPRegen();
                }

                if (m_movementAction.action.ReadValue<Vector2>().sqrMagnitude > 0)
                {
                    MoveTo(m_movementAction.action.ReadValue<Vector2>());
                }
            }
        }

        public void OnBasicSkill(InputAction.CallbackContext c)
        {
            if (m_basicSkill.CurrentSkill == null || 
                !m_basicSkill.CurrentSkill.IsUsable || !IsReady) 
            {
                return;
            }

            if (c.interaction is PressInteraction)
            {
                switch (c.phase)
                {
                    case InputActionPhase.Performed:
                        if (!Mouse.current.rightButton.isPressed)
                        {
                            Vector2 pos = Character.CurrentCamera.
                                ScreenToWorldPoint(Mouse.current.position.ReadValue());
                            OnBasicSkill(pos);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        protected override IEnumerator OnSkill(Vector2 target)
        {
            // trigger dash windup anim
            m_anim.SetBool("DashCharging", true);
            m_anim.SetTrigger("DashTrigger");
            yield return new WaitForSeconds(m_basicSkill.CurrentSkill.Properties.WindupTime);
            m_anim.SetBool("DashCharging", false);
            m_basicSkill.ActivateSkill(target);
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
        #endregion

        public void UseItem(InputAction.CallbackContext c) 
        {
            if (c.interaction is TapInteraction)
            {
                switch (c.phase)
                {
                    case InputActionPhase.Started:
                        if (!Mouse.current.rightButton.isPressed)
                        {
                            int slot = (int)c.ReadValue<float>();
                            Character.UseItem(slot - 1);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void InterruptSkill()
        {
            base.InterruptSkill();
            // Interrupt the input stun and reapply the stun timer
            m_anim.SetTrigger("TakeDamage");
        }
    }
}
