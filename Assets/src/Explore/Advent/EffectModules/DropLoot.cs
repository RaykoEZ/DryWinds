using Curry.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class DropLoot : PropertyAttribute
    {
        [SerializeField] List<LootDrop> m_lootPool = default;
        public virtual void ApplyEffect(LootManager lootManager)
        {
            LootDrop drop = SamplingUtil.SampleWithWeights(m_lootPool);
            lootManager.ReceiveLoot(drop.LootItems);
        }
    }
}
