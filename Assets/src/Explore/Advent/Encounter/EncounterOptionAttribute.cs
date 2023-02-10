using Curry.UI;
using System;
using UnityEngine;
using Curry.Util;
using UnityEngine.UIElements;

namespace Curry.Explore
{
    [Serializable]
    public class EncounterOptionAttribute : PropertyAttribute
    {
        [TextArea]
        [SerializeField] string m_description;
        [TextArea]
        [SerializeField] string m_summary;
        [SerializeField] EncounterResultAttribute m_encounterResult;
        public string Description => m_description;
        public string Summary => m_summary;
        public EncounterResultAttribute Result => m_encounterResult;
        public EncounterOptionAttribute(string description, string detail, EncounterResultAttribute result) 
        {
            m_description = description;
            m_summary = detail;
            m_encounterResult = result;
        }
    }
}