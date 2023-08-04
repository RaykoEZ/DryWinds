using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class DiscardCard_Random : PropertyAttribute
    {
        [SerializeField] int m_numToDiscard = default;
        public void ApplyEffect(HandManager hand, int numToDiscard)
        {
            hand.DiscardRandom(numToDiscard);
        }
        public void ApplyEffect(HandManager hand)
        {
            hand.DiscardRandom(m_numToDiscard);

        }
    }
}