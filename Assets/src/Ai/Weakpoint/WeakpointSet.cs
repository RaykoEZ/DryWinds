using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public delegate void OnBodyPartHit(BodyPart bodyPart);
    // A set of npc weakpoints to animate
    [Serializable]
    public class WeakpointSet
    {
        [SerializeField] List<BodyPart> m_weakPoints = default;
        public event OnBodyPartHit OnBodyPartBreak;
        public event OnBodyPartHit OnWeakpointBreak;
        public event OnCharacterTakeDamage OnTakeDamage;
        public virtual void Show()
        {
            foreach(BodyPart weakPoint in m_weakPoints) 
            {
                weakPoint.ShowWeakpoint();
            }
        }

        public virtual void Hide()
        {
            foreach (BodyPart weakPoint in m_weakPoints)
            {
                weakPoint.HideWeakpoint();
            }
        }
    }
}
