using System;
using UnityEngine;
using Curry.Events;
using System.Collections;

namespace Curry.Explore
{
    // Need to listen to win detector, if won/loss, => GaemEnd
    [Serializable]
    public class PlayerAction : Phase
    {
        [SerializeField] CurryGameEventTrigger m_onTurnStart = default;
        [SerializeField] EndTurnTrigger m_turnEnd = default;
        public override void Init()
        {
            NextState = typeof(TurnEnd);
            m_turnEnd.OnTurnEnd += TransitionTo;
        }
        public override void Pause()
        {
            m_turnEnd.SetInteractable(false);
        }
        public override void Resume()
        {
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