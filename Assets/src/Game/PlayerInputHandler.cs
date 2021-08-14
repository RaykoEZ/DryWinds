using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Curry.Game
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] Player m_player = default;
        [SerializeField] SkillHandler m_skillHandler = default;
        [SerializeField] AnimatorHandler m_anim = default;
        [SerializeField] InputActionReference m_movementAction = default;

        protected Coroutine m_currentInputRecovery;
        protected bool m_disableInput = false;

        void Start()
        {
            m_skillHandler.Init(m_player);
            m_player.OnHitStun += OnHitStun;
        }

        void Update()
        {
            if (!m_disableInput) 
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    Vector2 pos = m_player.CurrentCamera.
                        ScreenToWorldPoint(Mouse.current.position.ReadValue());

                    m_player.CurrentBrush.Draw(m_player.CurrentStats, pos);
                }

                if (m_movementAction.action.ReadValue<Vector2>().sqrMagnitude > 0)
                {
                    OnMovement(m_movementAction.action.ReadValue<Vector2>());
                }
            }
        }

        public void OnMovement(Vector2 dir) 
        {
            m_player.RigidBody.AddForce(dir * m_player.CurrentStats.Speed, ForceMode2D.Force);           
        }

        public void OnDashTrigger(InputAction.CallbackContext c)
        {
            if (!m_skillHandler.IsSkillAvailable || m_disableInput) 
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
                            m_skillHandler.SkillWindup();
                        }
                        else
                        {
                            m_anim.OnDashRelease();
                            Vector2 pos = m_player.CurrentCamera.
                                ScreenToWorldPoint(Mouse.current.position.ReadValue());
                            // Dash when rmb released
                            m_skillHandler.ActivateSkill(pos);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnDrawTrigger(InputAction.CallbackContext c)
        {
            if (c.interaction is PressInteraction)
            {
                switch (c.phase)
                {
                    case InputActionPhase.Performed:
                        // Finished a brush stroke
                        m_player.CurrentBrush.OnTraceEnd();
                        break;
                    default:
                        break;
                }
            }
        }
      
        protected void OnHitStun(float damage) 
        {
            // Interrupt the input stun and reapply the stun timer
            if (m_currentInputRecovery != null) 
            {
                StopCoroutine(m_currentInputRecovery);
            }
            m_skillHandler.InterruptWindup();
            m_anim.OnTakeDamage();
            m_currentInputRecovery = StartCoroutine(RecoverInput());
        }

        protected virtual IEnumerator RecoverInput() 
        {
            m_disableInput = true;
            yield return new WaitForSeconds(m_player.CurrentStats.HitRecoveryTime);
            m_disableInput = false;
            m_currentInputRecovery = null;
        }

    }
}
