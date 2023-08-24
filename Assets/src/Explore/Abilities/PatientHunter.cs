using Curry.Util;
using Curry.Vfx;
using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public interface IEnemyReaction
    {
        bool CanReact(IEnemy user);
        AbilityContent AbilityDetail { get; }
        public IEnumerator OnPlayerAction(IEnemy enemy);
    }
    [Serializable]
    public class PatientHunter : BaseAbility, IStackableEffect, IEnemyReaction
    {
        [SerializeField] TakeAim m_preparedBuff = default;
        bool m_activated = false;
        TakeAim m_instance;
        VfxManager.VfxHandle m_buffVfx;
        public int CurrentStack => m_instance != null ? m_instance.CurrentStack : 0;

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
                m_instance = new TakeAim(m_preparedBuff);
                applyTo.ApplyModifier(m_instance, m_instance.Vfx, m_instance.VfxTimeline, out m_buffVfx);
                m_buffVfx?.PlayVfx();
                m_activated = true;
            }
            else if (m_instance.CurrentStack < m_instance.MaxStack)
            {
                m_buffVfx?.PlayVfx();
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
                m_buffVfx?.StopVfx();
                m_buffVfx = null;
                m_activated = false;
            }
            return ret;
        }
    }
}
