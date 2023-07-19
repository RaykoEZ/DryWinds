using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public struct LootDrop : IWeightedItem
    {
        [SerializeField] int m_oddsWeight;
        [SerializeField] List<CardAsset> m_lootItems;
        public int Weight => m_oddsWeight;
        public List<CardAsset> LootItems => m_lootItems;
    }
}
