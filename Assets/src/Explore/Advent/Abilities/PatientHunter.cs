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
        protected void Activate(ICharacter user)
        {
            IModifiable applyTo = user as IModifiable;
            if (applyTo == null) { return; }

            if (!m_activated) 
            {
                OnMessage?.Invoke($"{AbilityDetail.Name} Activated");
                m_instance = new TakeAim(m_preparedBuff);
                applyTo?.ApplyModifier(m_instance, m_instance.Vfx, m_instance.VfxTimeline);
                m_activated = true;
            }
            else
            {
                user.TriggerVfx(Vfx, VfxTimeline);
                m_instance.AddStack();
            }
        }
        public IEnumerator OnPlayerAction(IEnemy enemy)
        {
            Activate(enemy);         
            yield return null;
        }
        public bool CanReact(IEnemy user)
        {
            bool ret = user.SpotsTarget;
            // reset modifier
            if (m_activated && !ret)
            {
                (user as IModifiable).CurrentStats.RemoveModifier(m_instance);
                m_activated = false;
            }
            return ret;
        }
    }
}
