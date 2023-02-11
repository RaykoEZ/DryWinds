﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "AddToHand_", menuName = "Curry/Encounter/Effects/Add Card(s)/To Hand", order = 3)]
    public class AddCardsToHand : EncounterEffect, IAddToHandModule
    {
        [SerializeField] List<AdventCard> m_cardsToAdd = default;

        public string ModuleName => nameof(m_cardsToAdd);

        public override IEnumerator Activate(GameStateContext context)
        {
            context.Deck.AddToHand(m_cardsToAdd);
            yield return new WaitForEndOfFrame();
        }
    }
}