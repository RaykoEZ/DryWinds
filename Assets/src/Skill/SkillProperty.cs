using System;
using Curry.Game;

namespace Curry.Skill
{

    [Serializable]
    public struct SkillProperty : IActionProperty
    {
        public string Name { get { return m_name; } }
        public ObjectRelations TargetOptions { get { return m_targetOptions; } }
        public float SpCost { get { return m_spCost; } }
        public float CooldownTime { get { return m_cooldownTime; } }
        public float MaxWindupTime { get { return m_maxWindupTime; } }
        public float SkillValue { get { return m_skillValue; } }
        public float Knockback { get { return m_knockback; } }

        public string m_name;
        public ObjectRelations m_targetOptions;
        public float m_spCost;
        public float m_cooldownTime;
        public float m_maxWindupTime;
        public float m_skillValue;
        public float m_knockback;
    }
}
