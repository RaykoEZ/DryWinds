using System;
using UnityEngine;
using Curry.Game;
using Curry.Events;
using System.Collections;
using System.Collections.Generic;
using Curry.UI;

namespace Curry.Explore
{
    public enum CardType 
    { 
        Skill = 0,
        Equipment = 1,
        Item = 2
    }
    [Serializable]
    public struct CardAttribute 
    {
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        [Range(0, 1000)]
        [SerializeField] public int TimeCost;
        [SerializeField] public CardType Type;
    }
    // Base class for all playable cards
    public abstract class AdventCard : PoolableBehaviour, IPoolable
    {
        [SerializeField] CardAttribute m_attribute = default;
        [SerializeField] protected List<CardResourceModule> m_resourceModules = default;
        bool m_activatable = true;
        public int Id => $"{m_attribute.Name}/{gameObject.name}".GetHashCode();
        public string Name => m_attribute.Name;
        public string Description => m_attribute.Description;
        public int TimeCost => m_attribute.TimeCost;
        public CardType Type => m_attribute.Type;
        // Whether keep card upon moving to a new tile
        public virtual bool Activatable { get { return m_activatable; } private set { m_activatable = value; } }
        public override void Prepare() 
        {
            Activatable = true;
            GetComponent<CardTextHandler>()?.SetCardText(Description);
            foreach(CardResourceModule resource in m_resourceModules) 
            {
                resource?.Init();
            }
        }
        // Card Effect
        public abstract IEnumerator ActivateEffect(IPlayer user);
    }
}
