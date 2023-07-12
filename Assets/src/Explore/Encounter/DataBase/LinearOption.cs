using System;
using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Curry.Explore
{
    // Stores the operations and operands for an encounter's option
    [Serializable]
    [CreateAssetMenu(fileName = "EncOpt_", menuName = "Curry/Encounter/Options/New Linear Option", order = 1)]
    public class LinearOption : EncounterOption, IEncounterModule
    {
        [SerializeField] EncounterContent m_outcome = default;
        public override EncounterContent GetOutcomeContent()
        {
            return m_outcome;
        }
    }
}