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
    }
}