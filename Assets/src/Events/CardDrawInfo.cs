using System.Collections.Generic;
using Curry.Events;
namespace Curry.Explore
{
    public class CardDrawInfo : EventInfo
    { 
        // All basic cards drawn (excludes all subclass AdventCard instances
        // e.g. Encounters 
        public IReadOnlyList<AdventCard> CardsDrawn { get; protected set; }
        public CardDrawInfo SetCardDraw(List<AdventCard> draw) 
        {
            if (draw != null) 
            {
                CardsDrawn = draw;
            }
            return this;
        }
    }

}
