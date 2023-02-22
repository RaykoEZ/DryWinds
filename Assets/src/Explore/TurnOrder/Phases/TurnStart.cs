using Curry.UI;
using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    public class TurnStart : Phase
    {
        [SerializeField] ChoiceConditions m_condition = default;
        [SerializeField] DeckManager m_deck = default;
        protected override Type NextState => typeof(PlayerAction);
        protected override IEnumerator Evaluate_Internal()
        {
            m_deck.ChooseToAddFromInventory(
                m_condition, 
                cardPoolFilter: null, 
                onChosen: TransitionTo);
            yield return null;
        }     
    }

    public static class ChoiceUtil 
    { 
        // create a list of choices from a list of cards instances
        public static List<IChoice> ChooseCards_FromInstance(IReadOnlyList<AdventCard> cardRefs)
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