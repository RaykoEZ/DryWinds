using Curry.Util;
using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class PatientHunter : BaseAbility, IStackableEffect, IEnemyReaction
    {
        [SerializeField] TakeAim m_preparedBuff = default;
        bool m_activated = false;
        TakeAim m_instance;
        public int CurrentStack => m_instance != null ? m_instance.CurrentStack : 0;
        public event OnAbilityMessage OnMessage;

        public override AbilityContent AbilityDetail => new AbilityContent
        { 
            Name = m_resource.Content.Name,
            Description = $"{m_resource.Content.Description} (Current Stack: {CurrentStack})",
            TargetingRange = RangeMapping.GetRangeMap(3),
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
        protected void Activate(ICharacter user, IModifiable applyTo, bool activate)
        {
            if (!m_activated && activate) 
            {
                OnMessage?.Invoke($"{AbilityDetail.Name} Activated");
                user.TriggerVfx(Vfx, VfxTimeline);
                m_instance = new TakeAim(m_preparedBuff);
                applyTo?.CurrentStats.ApplyModifier(m_instance);
                m_activated = true;
            }
            else if (m_activated && activate)
            {
                user.TriggerVfx(Vfx, VfxTimeline);
                m_instance.AddStack();
            }
            else if (m_activated && !activate)
            {
                applyTo.CurrentStats.RemoveModifier(m_instance);
                m_activated = false;
            }
        }
        public IEnumerator OnPlayerAction(IEnemy enemy)
        {
            if (enemy is IModifiable mod) 
            {
                Activate(enemy, mod, enemy.SpotsTarget);
            }
            yield return null;
        }
    }
}
