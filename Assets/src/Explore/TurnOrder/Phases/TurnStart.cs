using Curry.Game;
using Curry.UI;
using Curry.Util;
using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public class TurnStart : Phase
    {
        [SerializeField] ActionCounter m_actionCount = default;
        protected override Type NextState { get; set; } = typeof(PlayerAction);
        protected override IEnumerator Evaluate_Internal()
        {
            m_actionCount.UpdateMoveCountDisplay(m_actionCount.CurrentActionCount + 1);
            TransitionTo();
            yield return null;
        }
    }
}