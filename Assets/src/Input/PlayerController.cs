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
        [SerializeField] MovementHandler m_movementController = default;
        [SerializeField] Animator m_anim = default;
        float m_dashChargeTime = 0f;
        void Update()
        {
            // When rmb held, charge dash guage
            if (Mouse.current.rightButton.isPressed) 
            {
                m_dashChargeTime += 2f * Time.deltaTime;
            }

            if (Mouse.current.leftButton.isPressed)
            {
                m_player.CurrentBrush.Draw(m_player.Stats);
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
                        }
                        else 
                        {
                            // Dash when rmb released
                            m_movementController.Dash(m_player.Stats.Speed, m_dashChargeTime);
                            m_dashChargeTime = 0f;
                            
                            // trigger dash release anim
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
