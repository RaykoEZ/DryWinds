using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public struct EncounterDetail 
    {
        [SerializeField] string m_title;
        [SerializeField] string m_description;
        [SerializeField] Sprite m_coverImage;
        [SerializeField] List<EncounterOption> m_options;
        public string Title => m_title;
        public string Description => m_description;
        public Sprite CoverImage => m_coverImage;
        public IReadOnlyList<EncounterOption> Options => m_options;
    }
}