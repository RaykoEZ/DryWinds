using Curry.Util;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class PatientHunter : BaseAbility, IStackableEffect, IEnemyReaction
    {
        [SerializeField] Prepared m_preparedBuff = default;
        bool m_activated = false;
        Prepared m_instance;
        public override AbilityContent AbilityDetail => new AbilityContent
        { 
            Name = m_resource.Content.Name,
            Description = m_resource.Content.Description,
            TargetingRange =RangeMapping.GetRangeMap(3),
            Icon = m_resource.Content.Icon
        };

        public void AddStack(int addVal = 1) 
        {
            m_instance?.AddStack(addVal);
        }
        public void SubtractStack(int subVal = 1)
        {
            m_instance?.SubtractStack(subVal);
        }
        public void ResetStack() 
        {
            m_instance?.ResetStack();
        }
        // Activate this once on spawn
        protected void Activate(IModifiable applyTo, bool activate)
        {
            if (!m_activated && activate) 
            {
                m_instance = new Prepared(m_preparedBuff);
                applyTo?.CurrentStats.ApplyModifier(m_instance);
                m_activated = true;
            }
            else if (m_activated && activate)
            {
                m_instance.AddStack();
            }
            else if(m_activated && !activate)
            {
                applyTo.CurrentStats.RemoveModifier(m_instance);
                m_activated = false;
            }
        }
        public void OnPlayerAction(IEnemy enemy)
        {
            if(enemy is IModifiable mod) 
            {
                Activate(mod, enemy.SpotsTarget);
            }
        }
    }
}
