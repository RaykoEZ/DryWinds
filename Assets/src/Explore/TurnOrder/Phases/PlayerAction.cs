using System;
using UnityEngine;
namespace Curry.Explore
{
    // Need to listen to win detector, if won/loss, => GaemEnd
    [Serializable]
    public class PlayerAction : Phase
    {
        [SerializeField] PlayerInputController m_inputControl = default;
        [SerializeField] EndTurnTrigger m_turnEnd = default;
        public override void Init()
        {
            NextState = typeof(EnemyAction);
            m_turnEnd.OnTurnEnd += TransitionTo;
        }
        public override void Pause()
        {
            m_turnEnd.SetInteractable(false);
            m_inputControl.DisableInput();
        }
        public override void Resume()
        {
            m_turnEnd.SetInteractable(true);
            m_inputControl.EnableInput();
        }
        protected override void Evaluate()
        {
            Debug.Log("Player Action");
            m_turnEnd.SetInteractable(true);
            m_inputControl.EnableInput();
        }
        protected override void TransitionTo()
        {
            Debug.Log("Player Ends Turn");
            m_turnEnd.SetInteractable(false);
            m_inputControl.DisableInput();
            base.TransitionTo();
        }
    }
}