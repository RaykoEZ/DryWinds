using System;
using UnityEngine;
using Curry.Game;
using Curry.Events;
using System.Collections;

namespace Curry.Explore
{
    [Serializable]
    public struct CardAttribute 
    {
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        [Range(0, 1000)]
        [SerializeField] public int TimeCost;
    }
    // Base class for all playable cards
    public abstract class AdventCard : PoolableBehaviour, IPoolable
    {
        [SerializeField] CardAttribute m_attribute = default;
        bool m_activatable = true;
        public int Id { get { return $"{m_attribute.Name}/{gameObject.name}".GetHashCode(); } }
        public string Name { get { return m_attribute.Name; } }
        public string Description { get { return m_attribute.Description; } }
        public int TimeCost { get { return m_attribute.TimeCost; } }
        // Whether keep card upon moving to a new tile
        public virtual bool Activatable { get { return m_activatable; } private set { m_activatable = value; } }
        public override void Prepare() 
        {
            Activatable = true;
        }
        // Card Effect
        public abstract IEnumerator ActivateEffect(IPlayer user);

    }
}
