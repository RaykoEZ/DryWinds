﻿using Curry.Game;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [RequireComponent(typeof(CooldownAnimationHandler))]
    public class CooldownModule : CardResourceModule, ICooldown
    {
        [SerializeField] protected int m_cooldownTime = default;
        [SerializeField] protected bool m_isInitiallyOnCooldown = default;
        protected CooldownAnimationHandler CooldownAnim => GetComponent<CooldownAnimationHandler>();
        protected int m_current = 0;
        public bool IsOnCooldown { get; protected set; }
        public int CooldownTime { get => m_cooldownTime; set => m_cooldownTime = value; }
        public int Current { get => m_current; set => m_current = value; }
        public override void Init()
        {
            IsOnCooldown = m_isInitiallyOnCooldown;
            AnimateCooldown();
            m_current = 0;
        }
        public void TrggerCooldown()
        {
            m_current = m_cooldownTime;
            IsOnCooldown = true;
            AnimateCooldown();
        }
        public void Tick(int dt, out bool isOnCoolDown)
        {
            m_current -= dt;
            isOnCoolDown = m_current <= 0;
            AnimateCooldown();
        }
        protected virtual void AnimateCooldown() 
        {
            CooldownAnim?.OnCooldown(IsOnCooldown);
        }
    }
}
