using System;
using UnityEngine;
using Curry.Game;
using Curry.Events;

namespace Curry.Explore
{
    [Serializable]
    public struct CardAttribute 
    {
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        [Range(0, 1000)]
        [SerializeField] public int TimeCost;
        [SerializeField] public bool RetailCard;
    }
    // Base class for all playable cards
    public abstract class AdventCard : MonoBehaviour, IPoolable
    {
        [SerializeField] CardAttribute m_attribute = default;
        [SerializeField] CurryGameEventTrigger m_cardActivated = default;
        bool m_activatable = true;
        public int Id { get { return $"{m_attribute.Name}/{gameObject.name}".GetHashCode(); } }
        public string Name { get { return m_attribute.Name; } }
        public string Description { get { return m_attribute.Description; } }
        public int TimeCost { get { return m_attribute.TimeCost; } }
        public virtual bool RetainCard { get { return m_attribute.RetailCard; } }
        public virtual Action<AdventurerStats> CardEffect { get { return ActivateEffect; } }
        // Whether keep card upon moving to a new tile
        public virtual bool Activatable { get { return m_activatable; } protected set { m_activatable = value; } }
        public IObjectPool Origin { get; set; }

        public virtual void Prepare() 
        {
            Activatable = true;
        }
        public virtual void ReturnToPool()
        {
            ObjectPool<AdventCard>.ReturnToPool(Origin, this);
        }
        public virtual void OnDiscard()
        {
            OnExpend();
        }
        // Card Effect
        protected virtual void ActivateEffect(AdventurerStats user) 
        {
            m_cardActivated?.TriggerEvent(new EventInfo());
        }

        // After activating card, maybe expend the card
        protected virtual void OnExpend() 
        {
            ReturnToPool();
        }
    }
}
