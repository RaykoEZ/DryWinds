using Curry.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public struct EncounterDetail 
    {
        [SerializeField] string m_title;
        [TextArea]
        [SerializeField] string m_description;
        [SerializeField] Sprite m_coverImage;
        [SerializeField] List<OptionDetail> m_options;
        public string Title => m_title;
        public string Description => m_description;
        public Sprite CoverImage => m_coverImage;
        public IReadOnlyList<OptionDetail> Options => m_options;
    }
    [Serializable]
    public struct OptionDetail
    {
        [TextArea]
        [SerializeField] string m_description;
        [TextArea]
        [SerializeField] string m_summary;
        [SerializeField] EncounterResult m_encounterResults;
        public string Description => m_description;
        public string Summary => m_summary;
        public EncounterResult Result => m_encounterResults;
        public OptionDetail(string description, string detail, EncounterResult result) 
        {
            m_description = description;
            m_summary = detail;
            m_encounterResults = result;
        }


    }

}