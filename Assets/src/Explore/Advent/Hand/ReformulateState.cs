using System.Collections.Generic;

namespace Curry.Explore
{
    public class ReformulateState 
    {
        public List<AdventCard> HandCards;
        public List<AdventCard> InventoryCards;
        public void Clear() 
        {
            HandCards.Clear();
            InventoryCards.Clear();
        }
        public ReformulateState() 
        {
            HandCards = new List<AdventCard>();
            InventoryCards = new List<AdventCard>();
        }
        public ReformulateState(ReformulateState copy) 
        {
            HandCards = new List<AdventCard>(copy.HandCards);
            InventoryCards = new List<AdventCard>(copy.InventoryCards);
        }
    }
}