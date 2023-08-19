using System;
using UnityEngine;
using System.Collections;

namespace Curry.Explore
{
    public class EnemyAction : Phase
    {
        protected override Type NextState { get; set; } = typeof(TurnEnd);
        protected override IEnumerator Evaluate_Internal()
        {
            yield return null;
            TransitionTo();
        }
    }
}