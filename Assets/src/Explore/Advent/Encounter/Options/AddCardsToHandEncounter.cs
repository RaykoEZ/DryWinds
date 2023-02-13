using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "AddToHand_", menuName = "Curry/Encounter/Effects/Add Card(s)/To Hand", order = 3)]
    public class AddCardsToHandEncounter : EncounterEffect, IEncounterModule
    {
        [SerializeField] AddCardToHand m_cardsToAdd = default;
        public override string[] SerializePropertyNames => new string[] { nameof(m_cardsToAdd) };

        public override IEnumerator Activate(GameStateContext context)
        {
            m_cardsToAdd.ApplyEffect(context.Deck);
            yield return new WaitForEndOfFrame();
        }
    }
}