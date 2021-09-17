using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;
namespace Curry.Game
{
    public interface IScorable<T> where T : IScoringArgs
    {
        public float Evaluate(T priorityVal);
    }

    public interface IScoringArgs 
    { 
        float BasePriority { get; }
    }

    [Serializable]
    public struct SurvivalArgs : IScoringArgs 
    {
        [SerializeField] float m_priority;
        CharacterStats m_stats;
        public float BasePriority { get { return m_priority; } }
        public CharacterStats CurrentStats { get { return m_stats; } }
        public SurvivalArgs(float priority, CharacterStats stats) 
        {
            m_priority = priority;
            m_stats = stats;
        }

    }

    [Serializable]
    public struct OffenseArgs : IScoringArgs
    {
        [SerializeField] float m_priority;
        CharacterStats m_targetStats;
        List<SkillProperty> m_skillValues;
        public float BasePriority { get { return m_priority; } }
        public CharacterStats TargetStats { get { return m_targetStats; } }
        public List<SkillProperty> SkillValues { get { return m_skillValues; } }

        public OffenseArgs(float priority, CharacterStats targetStats, List<SkillProperty> skillValues)
        {
            m_priority = priority;
            m_targetStats = targetStats;
            m_skillValues = skillValues;
        }

    }
}
