﻿using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{

    // Base class for all playable cards
    public abstract class AdventCard : MonoBehaviour, IPoolable
    {
        [SerializeField] protected int m_id = 0;
        [Range(0, 1000)]
        [SerializeField] protected int m_timeCost = default;
        [SerializeField] protected string m_name = default;
        [SerializeField] protected string m_description = default;
        public virtual bool Activatable { get; protected set; }
        public int Id { get { return m_id; } }
        public string Name { get { return m_name; } }
        public string Description { get { return m_description; } }
        public int TimeCost { get { return m_timeCost; } }
        public virtual Action<Explorer> CardEffect { get { return ActivateEffect; } }

        public IObjectPool Origin { get; set; }

        public abstract void Prepare();
        public virtual void ReturnToPool()
        {
            ObjectPool<AdventCard>.ReturnToPool(Origin, this);
        }

        // Card Effect
        protected abstract void ActivateEffect(Explorer user);
        // After activating card, maybe expend the card
        protected virtual void OnExpend() 
        {
            ReturnToPool();
        }
    }
}
