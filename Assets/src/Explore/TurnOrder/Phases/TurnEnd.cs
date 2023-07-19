using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public delegate void OnTurnEnd();
    public class TurnEnd : Phase
    {
        public event OnTurnEnd OnTurnEnding;
        protected override Type NextState { get; set; } = typeof(TurnStart);
        protected override IEnumerator Evaluate_Internal()
        {
            OnTurnEnding?.Invoke();
            TransitionTo();
            yield return null;
        }
    }

}