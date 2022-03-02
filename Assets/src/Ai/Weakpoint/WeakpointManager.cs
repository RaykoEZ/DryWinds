using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    // Decides which sets of weakpoints to animate
    [Serializable]
    public class WeakpointManager
    {
        [SerializeField] List<WeakpointSet> m_weakpointSets = default;
        [SerializeField] BaseNpc m_npc = default;
        public event OnBodyPartHit OnBodyPartBreak;
        public event OnBodyPartHit OnWeakpointBreak;
        public event OnCharacterTakeDamage OnTakeDamage;
        public virtual void Show() 
        {
            foreach (WeakpointSet weakpoints in m_weakpointSets) 
            {
                weakpoints.Show();
            }
        }

        public virtual void Hide() 
        {
            foreach (WeakpointSet weakpoints in m_weakpointSets)
            {
                weakpoints.Hide();
            }
        }
    }
}
