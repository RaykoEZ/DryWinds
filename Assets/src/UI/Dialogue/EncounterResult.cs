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
        [SerializeField] List<AdventCard> m_cardGain;
        [SerializeField] List<AdventCard> m_cardLoss;
        [SerializeField] GameConditionAttribute m_milestoneAchieved;

        public List<AdventCard> CardGain => m_cardGain;
        public List<AdventCard> CardLoss => m_cardLoss;
        public GameConditionAttribute MilestoneAchieved => m_milestoneAchieved;
        public List<string> Dialogue => m_dialogue;
        public EncounterResult(
            List<string> text,
            List<AdventCard> cardGain,
            List<AdventCard> cardLoss,
            GameConditionAttribute milestoneAchieved)
        {
            m_dialogue = text;
            m_cardGain = cardGain;
            m_cardLoss = cardLoss;
            m_milestoneAchieved = milestoneAchieved;
        }
    }
}
