using System;
using UnityEngine;
using Curry.Events;
using System.Collections;
using Curry.UI;
using System.Collections.Generic;

namespace Curry.Explore
{
    // Need to listen to win detector, if won/loss, => GaemEnd
    [Serializable]
    public class PlayerAction : Phase
    {
        [SerializeField] CurryGameEventTrigger m_onTurnStart = default;
        [SerializeField] EndTurnTrigger m_turnEnd = default;

        protected override Type NextState => typeof(EnemyAction);

        public override void Init()
        {
            m_turnEnd.OnTurnEnd += TransitionTo;
        }
        public override void OnEnter(Phase incomingState)
        {
            EndInterrupt();
            Evaluate();
        }
        public override void Pause()
        {
            StartInterrupt();
            m_turnEnd.SetInteractable(false);
        }
        public override void Resume()
        {
            EndInterrupt();
            m_turnEnd.SetInteractable(true);
        }

        protected override IEnumerator Evaluate_Internal()
        {
            m_onTurnStart?.TriggerEvent(new EventInfo());
            m_turnEnd.SetInteractable(true);
            yield return null;
        }

        protected override void TransitionTo()
        {
            m_turnEnd.SetInteractable(false);
            base.TransitionTo();
        }
    }
}