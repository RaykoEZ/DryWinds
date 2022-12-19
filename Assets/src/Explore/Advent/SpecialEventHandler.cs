using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    // These events contain event cards to draw once only (until replenished)
    public class SpecialEventHandler : MonoBehaviour
    {
        [SerializeField] protected List<AdventCard> m_eventCards = default;
        public IReadOnlyList<AdventCard> EventCards { get { return m_eventCards;} }
        public void AddEvent(AdventCard card) 
        {
            m_eventCards?.Add(card);
        }

        public void RemoveEvents() 
        {
            m_eventCards.Clear();
        }
    }

}
