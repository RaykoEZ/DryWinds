using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public class TurnEnd : Phase
    {      
        public override void Init()
        {
            NextState = typeof(TurnStart);
        }

        protected override IEnumerator Evaluate_Internal()
        {
            Debug.Log("Turn End");
            TransitionTo();
            yield return null;
        }
    }

}