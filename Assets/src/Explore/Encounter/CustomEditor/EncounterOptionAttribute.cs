using Curry.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;

namespace Curry.Explore
{
    [Serializable]
    public class EncounterOptionAttribute : PropertyAttribute
    {
        [TextArea]
        [SerializeField] string m_description = default;
        [SerializeField] EncounterOutcome m_outcomeDetail = default;
        public string Description => m_description;
        public EncounterOutcome OutcomeDetail => m_outcomeDetail;
        public EncounterOptionAttribute(string description, string detail, EncounterOutcome outcome) 
        {
            m_description = description;
            m_outcomeDetail = outcome;
        }
    }
}