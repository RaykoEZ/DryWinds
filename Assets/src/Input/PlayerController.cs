using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Animations;
using Curry.Game;

namespace Curry.Input
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Player m_player = default;
        [SerializeField] SkillHandler m_skillHandler = default;
        [SerializeField] AnimatorHandler m_anim = default;

        private void Start()
        {
            m_skillHandler.Init(m_player);
        }

        void Update()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                Vector2 pos = m_player.CurrentCamera.
                    ScreenToWorldPoint(Mouse.current.position.ReadValue());

                m_player.CurrentBrush.Draw(m_player.CurrentStats, pos);
            }
        }

        public void OnDashTrigger(InputAction.CallbackContext c)
        {
            if (c.interaction is PressInteraction)
            {
                switch (c.phase)
                {
                    case InputActionPhase.Performed:
                        if (Mouse.current.rightButton.isPressed)
                        {
                            // trigger dash windup anim on rmb press
                            m_anim.OnDashWindUp();
                            m_skillHandler.DashWindup();
                        }
                        else
                        {
                            m_anim.OnDashRelease();
                            Vector2 pos = m_player.CurrentCamera.
                                ScreenToWorldPoint(Mouse.current.position.ReadValue());
                            // Dash when rmb released
                            m_skillHandler.Dash(pos);
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

    }
}
