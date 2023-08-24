using Curry.UI;
using System.Collections.Generic;
namespace Curry.Explore
{
    public static class ChoiceUtil 
    { 
        // create a list of choices from a list of cards instances
        public static List<IChoice> ChooseCards(IReadOnlyList<AdventCard> cardRefs)
        {
            List<IChoice> choices = new List<IChoice>();
            foreach (AdventCard card in cardRefs)
            {
                if(card.TryGetComponent(out CardInteractionController choice)) 
                {
                    choices.Add(choice);
                }
                else 
                {
                    choices.Add(CardInteractionController.AttachToCard(card));
                }
            }
            return choices;
        }
        // Setup cards to be choices
        public static List<IChoice> SetupCardChoice(List<AdventCard> cardSource)
        {
            List<AdventCard> copies = new List<AdventCard>();
            // Instantiate copies
            // and assgn their Value property to the real card in inventory for later process
            foreach (AdventCard card in cardSource)
            {
                card.GetComponent<CardInteractionController>()?.
                    Init(card, CardInteractMode.Inspect | CardInteractMode.Select);
                copies.Add(card);
            }
            List<IChoice> ret = ChoiceUtil.ChooseCards(copies);
            return ret;
        }
    }
}