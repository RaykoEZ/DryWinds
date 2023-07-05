using UnityEngine;
using Curry.Game;
using System.Collections;
using System.Collections.Generic;
using Curry.UI;
using Curry.Util;

namespace Curry.Explore
{
    // Base class for all playable cards
    public abstract class AdventCard : PoolableBehaviour, IPoolable
    {
        [SerializeField] protected CardResource m_resources = default;
        [SerializeField] protected CooldownModule m_cooldown = default;
        [SerializeField] protected PositionTargetingModule m_targeting = default;
        [SerializeField] List<CardStartupModules> m_startupModulesModules = default;
        bool m_activatable = true;
        public int Id => $"{m_resources.Attribute.Name}/{gameObject.name}".GetHashCode();
        public string Name => m_resources.Attribute.Name;
        public string Description => m_resources.Attribute.Description;
        public RangeMap TargetingRange => m_resources.Attribute.TargetingRange;
        public ActionCost Cost => m_resources.Attribute.Cost;
        public int CooldownTime => m_resources.Attribute.Cooldown;
        public int HoldingValue => m_resources.Attribute.HoldingValue;
        public bool IsInitiallyOnCooldown => m_resources.Attribute.IsInitiallyOnCooldown;
        public bool IsOnCooldown => m_cooldown.IsOnCooldown;
        public int CurrentCooldown { get => m_cooldown.CurrentCooldown;}
        // Whether this card has satisfied activation conditions, if the card has any
        public virtual bool ConditionsSatisfied => true;

        // Whether keep card upon moving to a new tile
        public virtual bool IsActivatable(GameStateContext c)
        { return m_activatable; }         
        public override void Prepare() 
        {
            var content = GetComponent<CardContentSetter>();
            content?.Setup(m_resources.Attribute);

            if (this is ICooldown cd) 
            {
                content?.SetCooldown(cd.CooldownTime.ToString());
            }
            else 
            {
                content?.SetCooldown("-");
            }
            bool isConsume = this is IConsumable;
            content?.SetConsumableIcon(isConsume);
            foreach (CardStartupModules resource in m_startupModulesModules) 
            {
                resource?.Init(m_resources.Attribute);
            }
        }
        //Default Card Effect
        public abstract IEnumerator ActivateEffect(ICharacter user, GameStateContext context);
        // Cooldown operation, if a card has cooldown (follows ICooldown)
        public void TriggerCooldown()
        {
            ((ICooldown)m_cooldown).TriggerCooldown();
        }
        public void Tick(int dt, out bool isOnCoolDown)
        {
            ((ICooldown)m_cooldown).Tick(dt, out isOnCoolDown);
        }
        // If we need position targeting, use this
        public void SetTarget(Vector3 target)
        {
            m_targeting.SetTarget(target);
        }
    }
}
