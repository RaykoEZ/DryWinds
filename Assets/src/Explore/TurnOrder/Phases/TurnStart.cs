using System;
using System.Collections;
namespace Curry.Explore
{
    public class TurnStart : Phase
    {
        protected override Type NextState { get; set; } = typeof(PlayerAction);
        protected override IEnumerator Evaluate_Internal()
        {
            TransitionTo();
            yield return null;
        }
    }
}