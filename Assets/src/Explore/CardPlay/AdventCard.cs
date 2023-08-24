using UnityEngine;
using Curry.Game;
using System.Collections.Generic;
using Curry.Vfx;

namespace Curry.Explore
{
    // Base class for all playable cards
    public class AdventCard : PoolableBehaviour, IPoolable
    {
        [SerializeField] protected VfxSequencePlayer m_vfxHandler = default;
        [SerializeField] protected CooldownModule m_cooldown = default;
        [SerializeField] protected CardContentSetter m_contentSetter = default;
        [SerializeField] protected PositionTargetingModule m_targeting = default;
        [SerializeField] List<CardStartupModules> m_startupModulesModules = default;
        protected CardResource m_cardResource = default;
        bool m_activatable = true;
        public int Id => $"{m_cardResource.Name}/{gameObject.name}".GetHashCode();
        public virtual CardResource Resource => m_cardResource;
        // Whether keep card upon moving to a new tile
        public virtual bool IsActivatable(GameStateContext c)
        { return m_activatable && m_cardResource.ConditionsSatisfied; }
        public override void Prepare()
        {
        }
        public virtual void InitResource(CardResource resource) 
        {
            m_cardResource = resource;
            string cdText = resource.CooldownTime > 0? resource.CooldownTime.ToString() : "-";
            m_contentSetter?.SetCooldown(cdText);
            m_cardResource?.Init(m_cooldown, m_targeting);
            m_contentSetter?.SetConsumableIcon(Resource.ConsumedOnPlay);
            m_contentSetter?.Setup(m_cardResource.Properties);
            foreach (CardStartupModules startup in m_startupModulesModules)
            {
                startup?.Init(m_cardResource.Properties);
            }
        }
    }
}
