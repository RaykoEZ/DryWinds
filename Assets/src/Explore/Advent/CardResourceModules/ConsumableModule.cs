using Curry.Game;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class ConsumableModule : IConsumable
    {
        [SerializeField] protected int m_maxUses = default;
        protected PoolableBehaviour m_consumedObject;
        protected int m_usesLeft = 0;
        public int MaxUses => m_maxUses;
        public int UsesLeft => m_usesLeft;

        public void Init(PoolableBehaviour consume) 
        {
            m_consumedObject = consume;
            m_usesLeft = m_maxUses;
        }

        public void OnExpend()
        {
            m_consumedObject?.ReturnToPool();
        }

        public void OnUse()
        {
            m_usesLeft--;
            if (UsesLeft <= 0) 
            {
                OnExpend();
            }
        }

        public void SetMaxUses(int newMax)
        {
            m_maxUses = newMax;
        }

        public void SetUsesLeft(int newUsesLeft)
        {
            m_usesLeft = newUsesLeft;
        }
    }
}
