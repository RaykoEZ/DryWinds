using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    // Stores operator & operands (includes odds in weights) of possible outcomes of an encounter option
    [Serializable]
    [CreateAssetMenu(fileName = "RdEncOpt_", menuName = "Curry/Encounter/Options/New Randomized Option", order = 1)]
    public class RandomizedOption : EncounterOption, IEncounterModule
    {
        [SerializeField] List<WeightedEncounterContent> m_potentialOutcomes = default;
        public override EncounterContent GetOutcomeContent()
        {
            return SamplingUtil.SampleWithWeights(m_potentialOutcomes).Content;
        }
    }
}