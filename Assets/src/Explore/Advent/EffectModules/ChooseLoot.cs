using Curry.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class ChooseLoot : PropertyAttribute
    {
        [SerializeField] ChoiceConditions m_conditions = default;
        [SerializeReference] List<CardAsset> m_lootOptions = default;
        protected virtual List<AdventCard> OnLootChosen(ChoiceResult result) 
        {
            List<AdventCard> ret = new List<AdventCard>();
            foreach(IChoice choice in result.Chosen) 
            { 
                if (choice is MonoBehaviour behaviour && behaviour.TryGetComponent(out AdventCard card)) 
                {
                    ret.Add(card);
                }
            }
            return ret;
        }
        public virtual void ApplyEffect(DeckManager deck, LootManager lootManager)
        {
            OnChoiceFinish onFinish = (result) =>
            {
                List<AdventCard> loot = OnLootChosen(result);
                lootManager.ReceiveLoot(loot);
            };
            var cards = deck.InstantiateCards_FromAsset(m_lootOptions);
            List<IChoice> choices = ChoiceUtil.SetupCardChoice(cards);
            lootManager.ChooseLoot(choices, m_conditions, onFinish);
        }
    }
}