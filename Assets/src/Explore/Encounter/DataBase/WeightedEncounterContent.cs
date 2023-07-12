using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public interface IWeightedItem
    {
        int Weight { get; }
    }
    [Serializable]
    public class EncounterContent 
    {
        [SerializeField] List<BaseEffectResource> m_effects;
        [TextArea]
        [SerializeField] List<string> m_dialogue;
        public List<BaseEffectResource> Effects => m_effects;
        public List<string> Dialogue => m_dialogue;
    }
    [Serializable]
    public class WeightedEncounterContent : IWeightedItem
    {
        [Range(0, 100)]
        [SerializeField] int m_probabilityWeight;
        [SerializeField] EncounterContent m_content;
        public int Weight => m_probabilityWeight;
        public EncounterContent Content => m_content;
    }
}