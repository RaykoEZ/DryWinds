using Curry.Game;
using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class ConsumableModule : CardResourceModule, IConsumable
    {
        [SerializeField] protected int m_maxUses = default;
        protected int m_usesLeft = 0;
        public int MaxUses => m_maxUses;
        public int UsesLeft => m_usesLeft;

        public override void Init() 
        {
            m_usesLeft = m_maxUses;
        }
        public virtual IEnumerator OnExpend()
        {
            yield return null;
        }

        public virtual void OnUse()
        {
            m_usesLeft--;
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
