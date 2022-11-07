using UnityEngine;
namespace Curry.Explore
{
    public class TurnStart : Phase
    {
        public override void Init()
        {
            NextState = typeof(PlayerAction);
        }

        protected override void Evaluate()
        {
            Debug.Log("Turn Start");
        }
    }

}