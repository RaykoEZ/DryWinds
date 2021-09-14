using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Curry.Skill;

namespace Curry.Game
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] Player m_player = default;
        [SerializeField] SkillHandler m_basicSkillHandler = default;
        [SerializeField] SkillHandler m_drawSkillHandler = default;
        [SerializeField] AnimatorHandler m_anim = default;

        [SerializeField] InputActionReference m_movementAction = default;

        protected Coroutine m_currentInputRecovery;
        protected bool m_disableInput = false;

        void Start()
        {
            m_basicSkillHandler.Init(m_player);
            m_drawSkillHandler.Init(m_player);
            m_player.OnTakingDamage += OnHitStun;
            m_player.OnActionInterrupt += OnInterrupt;

        }

        void Update()
        {
            if (!m_disableInput) 
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    OnDraw();
                }

                if (m_movementAction.action.ReadValue<Vector2>().sqrMagnitude > 0)
                {
                    OnMovement(m_movementAction.action.ReadValue<Vector2>());
                }
            }
        }

        public void OnMovement(Vector2 dir) 
        {
            float drag = m_player.RigidBody.drag;
            m_player.RigidBody.AddForce( dir * m_player.CurrentStats.Speed * drag);           
        }

        public void OnBasicSkill(InputAction.CallbackContext c)
        {
            if (!m_basicSkillHandler.IsSkillAvailable || m_disableInput) 
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
                            m_basicSkillHandler.SkillWindup();
                        }
                        else
                        {
                            m_anim.OnDashRelease();
                            Vector2 pos = m_player.CurrentCamera.
                                ScreenToWorldPoint(Mouse.current.position.ReadValue());
                            // Dash when rmb released
                            m_basicSkillHandler.ActivateSkill(pos);
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
                        m_drawSkillHandler.InterruptSkill();
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnDraw() 
        {
            Vector2 pos = m_player.CurrentCamera.
                            ScreenToWorldPoint(Mouse.current.position.ReadValue());
            SkillParam param = new SkillParam(pos);
            m_drawSkillHandler.ActivateSkill(param);
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

        protected void OnHitStun(float damage) 
        {
            // Interrupt the input stun and reapply the stun timer
            if (m_currentInputRecovery != null) 
            {
                StopCoroutine(m_currentInputRecovery);
            }
            OnInterrupt();
            m_anim.OnTakeDamage();
            m_currentInputRecovery = StartCoroutine(RecoverInput());
        }

        protected void OnInterrupt() 
        {
            m_basicSkillHandler.InterruptSkill();
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
