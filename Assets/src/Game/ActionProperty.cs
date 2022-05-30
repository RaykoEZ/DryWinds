using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public struct ActionProperty
    {
        [SerializeField] string m_name;
        [SerializeField] float m_spCost;
        [SerializeField] float m_cooldownTime;
        [SerializeField] float m_actionValue;
        [SerializeField] float m_knockback;

        public string Name { get { return m_name; } }
        public float SpCost { get { return m_spCost; } }
        public float CooldownTime { get { return m_cooldownTime; } }
        public float ActionValue { get { return m_actionValue; } }
        public float Knockback { get { return m_knockback; } }

        public ActionProperty(
            string name, 
            float spCost,
            float cooldownTime,
            float actonValue,
            float knockback)
        {
            m_name = name;
            m_spCost = spCost;
            m_cooldownTime = cooldownTime;
            m_actionValue = actonValue;
            m_knockback = knockback;
        }
    }
}