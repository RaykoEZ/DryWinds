using Curry.Explore;
using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.UI
{
    [Serializable]
    public struct EncounterResult
    {
        [TextArea]
        [SerializeField] List<string> m_dialogue;
        [SerializeField] List<EncounterEffect> m_effects;
        [SerializeField] GameConditionAttribute m_milestoneAchieved;

        public List<EncounterEffect> Effects => m_effects;
        public GameConditionAttribute MilestoneAchieved => m_milestoneAchieved;
        public List<string> Dialogue => m_dialogue;
        public EncounterResult(
            List<string> text,
            List<EncounterEffect> effects,
            GameConditionAttribute milestoneAchieved)
        {
            m_dialogue = text;
            m_effects = effects;
            m_milestoneAchieved = milestoneAchieved;
        }
    }
}
