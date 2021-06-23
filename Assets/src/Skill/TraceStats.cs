using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Skill
{
    [Serializable]
    public struct TraceStats
    {
        public string Name;
        // SP Cost per unit length
        public float SpCostScale;
        public float Durability;
        public float ContactDamage;
        // 0 for immobile traces when interaction produce movement
        public float SpeedScale;
    }
}
