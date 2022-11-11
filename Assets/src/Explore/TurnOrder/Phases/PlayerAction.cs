using System;
using UnityEngine;
namespace Curry.Explore
{
    // Need to listen to win detector, if won/loss, => GaemEnd
    [Serializable]
    public class PlayerAction : Phase
    {
        [SerializeField] PlayerInputController m_inputControl = default;
        public override void Init()
        {
            NextState = typeof(EnemyAction);
        }
        public override void Pause()
        {
            m_inputControl.DisableInput();
        }
        public override void Resume()
        {
            m_inputControl.EnableInput();
        }
        protected override void Evaluate()
        {
            Debug.Log("Player Action");
            m_inputControl.EnableInput();
        }
        protected override Type TransitionTo()
        {
            m_inputControl.DisableInput();
            return base.TransitionTo();
        }
    }
}