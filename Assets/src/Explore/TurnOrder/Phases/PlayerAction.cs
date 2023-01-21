using System;
using UnityEngine;
using Curry.Events;
using System.Collections;
using Curry.UI;

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
            NextState = typeof(EnemyAction);
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
            StartInterrupt();
            m_onTurnStart?.TriggerEvent(new EventInfo());
            m_turnEnd.SetInteractable(true);
            EndInterrupt();
            yield return null;
        }

        protected override void TransitionTo()
        {
            m_turnEnd.SetInteractable(false);
            base.TransitionTo();
        }
    }

    public class EnemyAction : Phase
    {
        [SerializeField] EnemyManager m_enemies = default;
        public override void Init()
        {
            NextState = typeof(TurnEnd);
        }

        protected override IEnumerator Evaluate_Internal()
        {
            StartInterrupt();

            yield return null;
            TransitionTo();
            EndInterrupt();
        }
    }
}