using Curry.UI;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    public class LootManager : MonoBehaviour 
    {
        [SerializeField] DeckManager m_deck = default;
        [SerializeField] ChoicePrompter m_prompter = default;
        public void ReceiveLoot(List<AdventCard> items) 
        {
            if (items == null || items.Count == 0) return;
            m_deck.AddToHand(items);
        }
        public void ChooseLoot(List<IChoice> items, ChoiceConditions conditions, OnChoiceFinish onFinish) 
        {
            m_prompter.MakeChoice(conditions, items, onFinish);
        }
    }
}
