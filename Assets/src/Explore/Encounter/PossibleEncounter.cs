using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public struct PossibleEncounter : IWeightedItem
    {
        [SerializeField] int m_weight;
        [SerializeField] EncounterEntry m_entry;
        public int Weight => m_weight;
        public EncounterDetail Value => m_entry.Detail;
    }
}
