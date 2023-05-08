using System.Collections.Generic;
using UnityEngine;
using System;
using Curry.Util;

namespace Curry.Explore
{
    [Serializable]
    public class ReinforcementPool : ScriptableObject 
    {
        [SerializeField] ReinforcementList m_reinforcementPool = default;
        public IReadOnlyList<ReinforcementTarget> Pool => m_reinforcementPool.Targets;
        public List<ReinforcementTarget> GetRandomTargets(int numToGet, bool uniqueResults = true) 
        {
            return GameUtil.SampleFromList(m_reinforcementPool.Targets, numToGet, uniqueResults);
        }
    }
}
