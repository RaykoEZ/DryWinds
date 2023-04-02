using Curry.UI;
using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class Provision : Phase
    {
        [SerializeField] ChoiceConditions m_condition = default;
        [SerializeField] DeckManager m_deck = default;
        protected override Type NextState { get; set; } = typeof(TurnStart);
        protected override IEnumerator Evaluate_Internal()
        {
            m_deck.ChooseToAddFromInventory(
                m_condition,
                cardPoolFilter: null,
                onChosen: TransitionTo);
            yield return null;
        }
    }
}