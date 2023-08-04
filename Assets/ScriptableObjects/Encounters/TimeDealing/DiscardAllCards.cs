using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class DiscardAllCards : PropertyAttribute
    {
        public void Activate(HandManager hand)
        {
            hand.DiscardAll();
        }
    }
}