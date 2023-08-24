using Curry.Util;
using Curry.Vfx;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.VFX;

namespace Curry.Explore
{
    // Contains card effect implementation, targeting, cooldown, and card description & sprite
    [Serializable]
    public class CardResource 
    {
        [SerializeField] private bool m_consumedOnPlay = default;
        [SerializeField] protected CardProperties m_properties = default;
        // A vfx and director to insert into a card instance for effect sequence when activating a card
        [SerializeField] protected VisualEffectAsset m_vfx = default;
        [SerializeField] protected TimelineAsset m_vfxTimeLine = default;
        protected PositionTargetingModule m_targeting = default;
        protected CooldownModule m_cooldownState = default;
        bool m_activatable = true;
        public string Name => m_properties.Name;
        public string Description => m_properties.Description;
        public RangeMap TargetingRange => m_properties.TargetingRange;
        public ActionCost Cost => m_properties.Cost;
        public int CooldownTime => m_properties.Cooldown;
        public int HoldingValue => m_properties.HoldingValue;
        public bool ConsumedOnPlay => m_consumedOnPlay;
        public bool IsInitiallyOnCooldown => m_properties.IsInitiallyOnCooldown;
        public bool IsOnCooldown => m_cooldownState != null ? m_cooldownState.IsOnCooldown : false;
        public int CurrentCooldown => m_cooldownState != null ? m_cooldownState.CurrentCooldown : 0;
        // Whether this card has satisfied activation conditions, if the card has any
        public virtual bool ConditionsSatisfied => true;
        public virtual CardProperties Properties => m_properties;


        public CardResource(CardResource effect) 
        {
            m_activatable = effect.m_activatable;
            m_properties = effect.m_properties;
            m_vfx = effect.m_vfx;
            m_vfxTimeLine = effect.m_vfxTimeLine;
        }
        public virtual void Init(CooldownModule cd, PositionTargetingModule targeting) 
        {
            m_cooldownState = cd;
            m_targeting = targeting;
        }
        public virtual bool IsActivatable(GameStateContext c)
        { return m_activatable; }
        public virtual IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            yield return null;
        }
        // Cooldown operation, if a card has cooldown (follows ICooldown)
        public void TriggerCooldown()
        {
            m_cooldownState.TriggerCooldown();
        }
        public void Tick(int dt, out bool isOnCoolDown)
        {
            m_cooldownState.Tick(dt, out isOnCoolDown);
        }
        // If we need position targeting, use this
        public void SetTarget(Vector3 target)
        {
            m_targeting.SetTarget(target);
        }
    }
}