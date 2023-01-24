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
        [SerializeField] ChoicePrompter m_prompter = default;
        [SerializeField] List<AdventCard> m_cardsToChooseFrom = default;
        [SerializeField] HandManager m_hand = default;
        protected override Type NextState => typeof(PlayerAction);
        protected override IEnumerator Evaluate_Internal()
        {
            List<IChoice> choices = ChoiceUtil.ChooseCards(m_cardsToChooseFrom, out List<GameObject> instances);
            m_prompter.MakeChoice(m_condition, choices, OnCardChosen);
            yield return null;
        }

        void OnCardChosen(ChoiceResult result) 
        {
            if (result.Status == ChoiceResult.ChoiceStatus.Cancelled) 
            {
                TransitionTo();
                return;
            }
            List<AdventCard> cards = new List<AdventCard>();
            foreach(IChoice choice in result.Choices) 
            { 
                if(choice is CardChoice cardChoice) 
                {
                    cardChoice.DisplayChoice(m_hand.transform);
                    CardChoice.DetachFromCard(cardChoice);
                    cards.Add(cardChoice.Value as AdventCard);
                }
            }
            m_hand?.AddCardsToHand(cards);
            TransitionTo();
        }
    }

    public static class ChoiceUtil 
    { 
        // create a list of choices from a list of cards
        public static List<IChoice> ChooseCards(List<AdventCard> cardRefs, out List<GameObject> instances) 
        {
            List<IChoice> choices = new List<IChoice>();
            instances = new List<GameObject>();
            foreach(AdventCard card in cardRefs) 
            {
                CardChoice choice = CardChoice.Create(card.gameObject, out GameObject instance);
                choices.Add(choice);
                instances.Add(instance);
            }

            return choices;
        }
    }
}