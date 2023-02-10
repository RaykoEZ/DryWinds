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
        [SerializeField] List<EncounterOptionAttribute> m_options;
        public string Title => m_title;
        public string Description => m_description;
        public Sprite CoverImage => m_coverImage;
        public IReadOnlyList<EncounterOptionAttribute> Options => m_options;
    }

}