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
        [SerializeField] EncounterOption m_optionDetail = default;
        public string Description => m_description;
        public EncounterOption OptionDetail => m_optionDetail;
    }
}