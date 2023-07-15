using System;

namespace Curry.Explore
{
    [Serializable]
    public class ExhaustActionPoint 
    {
        public void ApplyEffect(ActionCounter actionCounter, out int numSpent) 
        {
            numSpent = actionCounter.CurrentActionPoint;
            actionCounter.UpdateMoveCountDisplay(0);
        }
    }
}
