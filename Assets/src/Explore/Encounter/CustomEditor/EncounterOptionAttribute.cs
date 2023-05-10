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
        [TextArea]
        [SerializeField] string m_summary = default;
        [SerializeField] EncounterResultAttribute m_encounterResult = default;
        [SerializeField] EncounterEffect m_effect = default;
        public string Description => m_description;
        public string Summary => m_summary;
        public EncounterResultAttribute Result => m_encounterResult;
        public EncounterEffect Effect => m_effect;
        public EncounterOptionAttribute(string description, string detail, EncounterResultAttribute result, EncounterEffect effect) 
        {
            m_description = description;
            m_summary = detail;
            m_encounterResult = result;
            m_effect = effect;
        }
    }
}