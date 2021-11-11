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
        protected int m_currentItemIdx = 0;

        public override Player Character { get { return m_player; } }
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
                    Move(m_movementAction.action.ReadValue<Vector2>());
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
            Character.Animator.SetBool("DashCharging", true);
            Character.Animator.SetTrigger("DashTrigger");
            yield return new WaitForSeconds(m_basicSkill.CurrentSkill.Properties.WindupTime);
            Character.Animator.SetBool("DashCharging", false);
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

        public void SelectItem(int val) 
        {
            if (val < HeldInventory.MaxItemCount) 
            {
                m_currentItemIdx = val;
            }
        }

        public void UseItem() 
        {
            ICollectable item = Character.HeldInventory.GetItem(m_currentItemIdx);
            if (item != null)
            {
                item.Use();
            }
        }

        protected override void OnInterrupt()
        {
            base.OnInterrupt();
            // Interrupt the input stun and reapply the stun timer
            Character.Animator.SetTrigger("TakeDamage");
        }
    }
}
