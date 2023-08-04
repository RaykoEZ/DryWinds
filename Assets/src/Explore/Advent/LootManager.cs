using Curry.UI;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    public class LootManager : SceneInterruptBehaviour 
    {
        [SerializeField] DeckManager m_deck = default;
        [SerializeField] ChoicePrompter m_prompter = default;
        OnChoiceFinish m_onFinish;
        public void ReceiveLoot_FromAsset(List<CardAsset> items) 
        {
            if (items == null || items.Count == 0) return;
            m_deck.AddToHand_FromAsset(items);
        }
        public void ReceiveLoot(List<AdventCard> items)
        {
            if (items == null || items.Count == 0) return;
            m_deck.AddToHand(items);
        }
        public void ChooseLoot(List<IChoice> items, ChoiceConditions conditions, OnChoiceFinish onFinish) 
        {
            StartInterrupt();
            m_onFinish = onFinish;
            m_prompter.MakeChoice(conditions, items, OnLootChosen);
        }
        void OnLootChosen(ChoiceResult result) 
        {
            m_onFinish?.Invoke(result);
            EndInterrupt();
        }
    }
}
