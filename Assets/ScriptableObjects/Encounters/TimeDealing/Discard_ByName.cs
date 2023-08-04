using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class Discard_ByName : PropertyAttribute
    {
        [SerializeField] protected string m_discardCardName;
        public void Activate(HandManager hand) 
        {
            hand.DiscardCardByName(m_discardCardName);
        }
        public void Activate(HandManager hand, string cardName)
        {
            hand.DiscardCardByName(cardName);
        }
    }
}