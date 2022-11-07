using System;
using UnityEngine;
namespace Curry.Explore
{
    // Need to listen to win detector, if won/loss, => GaemEnd
    [Serializable]
    public class PlayerAction : Phase
    {
        public override void Init()
        {
            NextState = typeof(EnemyAction);
        }

        protected override void Evaluate()
        {
            throw new System.NotImplementedException();
        }
    }

}