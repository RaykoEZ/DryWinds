using Curry.UI;
using Curry.Util;
using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public class TurnStart : Phase
    {
        [SerializeField] ChoiceConditions m_condition = default;
        [SerializeField] DeckManager m_deck = default;
        [SerializeField] AdventButton m_adventureButton = default;
        protected override Type NextState => typeof(PlayerAction);
        protected override IEnumerator Evaluate_Internal()
        {
            m_deck.ChooseToAddFromInventory(
                m_condition, 
                cardPoolFilter: null, 
                onChosen: TransitionTo);
            yield return null;
        }

        protected override void TransitionTo()
        {
            base.TransitionTo();
            m_adventureButton.CanMove = true;
        }
    }
}