using Curry.Game;
using Curry.UI;
using System;
using TMPro;
using UnityEngine;

namespace Curry.Explore
{
    [RequireComponent(typeof(CardInteractionController))]
    [RequireComponent(typeof(CooldownAnimationHandler))]
    public class CooldownModule : CardStartupModules, ICooldown
    {
        protected int m_cooldownTime = default;
        protected bool m_isInitiallyOnCooldown = default;
        protected CooldownAnimationHandler CooldownAnim => GetComponent<CooldownAnimationHandler>();
        protected CardInteractionController Interaction => GetComponent<CardInteractionController>();
        protected int m_current = 0;
        public bool IsOnCooldown { get; protected set; }
        public int CooldownTime { get => m_cooldownTime; set => m_cooldownTime = value; }
        public int Current { get => m_current; set => m_current = value; }
        public override void Init(CardAttribute card)
        {
            IsOnCooldown = card.IsInitiallyOnCooldown;
            m_cooldownTime = card.Cooldown;
            AnimateCooldown();
            m_current = 0;
        }
        public void TriggerCooldown()
        {
            m_current = m_cooldownTime;
            IsOnCooldown = true;
            CardInteractMode disablePlay = Interaction.InteractMode;
            disablePlay &= ~CardInteractMode.Play & CardInteractMode.Inspect;
            Interaction.SetInteractionMode(disablePlay);
            AnimateCooldown();
        }
        public void Tick(int dt, out bool isOnCoolDown)
        {
            m_current -= dt;
            isOnCoolDown = m_current > 0;
            IsOnCooldown = isOnCoolDown;
            AnimateCooldown();
            // Enable play when off cooldown
            if (!isOnCoolDown) 
            {
                CardInteractMode enablePlay = Interaction.InteractMode;
                enablePlay &= CardInteractMode.Play;
                Interaction.SetInteractionMode(enablePlay);
            }
        }
        protected virtual void AnimateCooldown() 
        {
            CooldownAnim?.OnCooldown(Current, IsOnCooldown);
        }
    }
}
