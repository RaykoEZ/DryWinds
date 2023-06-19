using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public struct LootDrop : IWeightedItem
    {
        [SerializeField] int m_oddsWeight;
        [SerializeField] List<AdventCard> m_lootItems;
        public int Weight => m_oddsWeight;
        public List<AdventCard> LootItems => m_lootItems;
    }
}
