using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public class TurnEnd : Phase
    {
        protected override Type NextState { get; set; } = typeof(TurnStart);

        protected override IEnumerator Evaluate_Internal()
        {
            TransitionTo();
            yield return null;
        }
    }

}