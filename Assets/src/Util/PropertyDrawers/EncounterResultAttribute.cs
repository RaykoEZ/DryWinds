using Curry.Explore;
using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.UI
{
    [Serializable]
    public class EncounterResultAttribute : PropertyAttribute
    {
        [SerializeField] GameConditionAttribute m_milestoneAchieved;
        [TextArea]
        [SerializeField] List<string> m_dialogue;
        public GameConditionAttribute MilestoneAchieved => m_milestoneAchieved;
        public List<string> Dialogue => m_dialogue;
        public EncounterResultAttribute(
            List<string> text,
            GameConditionAttribute milestoneAchieved)
        {
            m_dialogue = text;
            m_milestoneAchieved = milestoneAchieved;
        }
    }
}
