using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class TriggerCooldownInHand : PropertyAttribute
    {
        public void ApplyEffect(HandManager hand)
        {
            hand.TriggerAllCooldowns();
        }
    }
}