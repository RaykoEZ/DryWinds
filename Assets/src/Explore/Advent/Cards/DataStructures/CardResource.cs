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
        [SerializeField] protected CardProperties m_properties = default;
        // A vfx and director to insert into a card instance for effect sequence when activating a card
        [SerializeField] protected VisualEffectAsset m_vfx = default;
        [SerializeField] protected TimelineAsset m_vfxTimeLine = default;
        protected PositionTargetingModule m_targeting = default;
        protected CooldownModule m_cooldownState = default;
        // For controlling card Vfx to time effect activation and visuals
        protected VfxHandler m_vfxHandler = default;
        bool m_activatable = true;
        public string Name => m_properties.Name;
        public string Description => m_properties.Description;
        public RangeMap TargetingRange => m_properties.TargetingRange;
        public ActionCost Cost => m_properties.Cost;
        public int CooldownTime => m_properties.Cooldown;
        public int HoldingValue => m_properties.HoldingValue;
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
        public virtual void Init(CooldownModule cd, PositionTargetingModule targeting, VfxHandler vfxHandler) 
        {
            m_cooldownState = cd;
            m_targeting = targeting;
            m_vfxHandler = vfxHandler;
            // setup vfx for card vfx sequences 
            SetupVfxAsset(m_vfx, m_vfxTimeLine);
        }
        public virtual bool IsActivatable(GameStateContext c)
        { return m_activatable; }
        public virtual IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            yield return null;
        }
        protected void SetupVfxAsset(VisualEffectAsset vfx, TimelineAsset timeline) 
        {
            // setup vfx for card vfx sequences 
            if (m_vfxHandler != null)
            {
                m_vfxHandler.SetupAsset(vfx, timeline);
            }
        }
        // play vfx at a location with user gameobject as the parent
        protected virtual IEnumerator PlayVfx(ICharacter user, Vector3 target)
        {
            Transform origin = m_vfxHandler.transform.parent;
            m_vfxHandler.transform.SetParent(user.GetTransform(), false);
            m_vfxHandler.transform.position = target;
            yield return m_vfxHandler?.PlaySequence();
            m_vfxHandler.transform.SetParent(origin, false);
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