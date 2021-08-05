using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Animations;
using Curry.Game;
using Curry.Skill;

namespace Curry.Input
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Player m_player = default;
        [SerializeField] DashHandler m_dashController = default;
        [SerializeField] AnimatorHandler m_anim = default;

        void Update()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                m_player.CurrentBrush.Draw(m_player.Stats);
            }
        }


        public void OnDashTrigger(InputAction.CallbackContext c)
        {
            if (c.interaction is PressInteraction && m_dashController.IsDashAvailable)
            {
                switch (c.phase)
                {
                    case InputActionPhase.Performed:
                        if (Mouse.current.rightButton.isPressed)
                        {
                            // trigger dash windup anim on rmb press
                            m_anim.OnDashWindUp();
                            m_dashController.DashWindup();
                        }
                        else
                        {
                            m_anim.OnDashRelease();
                            // Dash when rmb released
                            m_dashController.Dash(m_player.Stats.Speed);
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
