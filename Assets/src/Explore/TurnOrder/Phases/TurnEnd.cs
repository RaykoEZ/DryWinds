using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public class TurnEnd : Phase
    {
        [SerializeField] ActionCounter m_actionCount = default;
        protected override Type NextState { get; set; } = typeof(TurnStart);
        protected override IEnumerator Evaluate_Internal()
        {
            m_actionCount.UpdateMoveCountDisplay(m_actionCount.CurrentActionCount + 1);
            TransitionTo();
            yield return null;
        }
    }

}