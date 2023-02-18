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
        // create a list of choices from a list of cards (instantiates new cards)
        public static List<IChoice> ChooseCards(IReadOnlyList<AdventCard> cardRefs, out List<GameObject> instances) 
        {
            List<IChoice> choices = new List<IChoice>();
            instances = new List<GameObject>();
            foreach(AdventCard card in cardRefs) 
            {
                CardChoice choice = CardChoice.Create(card, out GameObject instance);
                choices.Add(choice);
                instances.Add(instance);
            }

            return choices;
        }
        // create a list of choices from a list of cards instances
        public static List<IChoice> ChooseCards_FromInstance(IReadOnlyList<AdventCard> cardRefs)
        {
            List<IChoice> choices = new List<IChoice>();
            foreach (AdventCard card in cardRefs)
            {
                CardChoice choice = CardChoice.AttachToCard(card);
                choices.Add(choice);
            }

            return choices;
        }
    }
}