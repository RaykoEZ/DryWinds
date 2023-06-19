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
        protected override Type NextState { get; set; } = typeof(EnemyAction);
        public override void Init()
        {
            m_turnEnd.OnTurnEnd += TransitionTo;
        }
        protected override IEnumerator Evaluate_Internal()
        {
            m_onTurnStart?.TriggerEvent(new EventInfo());
            yield return null;
        }
    }
}