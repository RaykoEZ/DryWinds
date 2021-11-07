using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public struct ActionProperty
    {
        [SerializeField] string m_name;
        [SerializeField] ObjectRelations m_targetOptions;
        [SerializeField] float m_spCost;
        [SerializeField] float m_cooldownTime;
        [SerializeField] float m_windupTime;
        [SerializeField] float m_actionValue;
        [SerializeField] float m_knockback;

        public string Name { get { return m_name; } }
        public ObjectRelations TargetOptions { get { return m_targetOptions; } }
        public float SpCost { get { return m_spCost; } }
        public float CooldownTime { get { return m_cooldownTime; } }
        public float WindupTime { get { return m_windupTime; } }
        public float ActionValue { get { return m_actionValue; } }
        public float Knockback { get { return m_knockback; } }

        public ActionProperty(
            string name, 
            ObjectRelations targetOptions,
            float spCost,
            float cooldownTime,
            float maxWindupTime,
            float actonValue,
            float knockback)
        {
            m_name = name;
            m_targetOptions = targetOptions;
            m_spCost = spCost;
            m_cooldownTime = cooldownTime;
            m_windupTime = maxWindupTime;
            m_actionValue = actonValue;
            m_knockback = knockback;
        }
    }
}