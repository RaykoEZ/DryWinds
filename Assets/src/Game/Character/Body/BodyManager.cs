using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public struct BodyHitResult 
    {
        public bool PartBreak;
        public bool WeakpointBreak;
        public float Damage;
        public int PartDamage;
        public float KnockbackMod;
        public Vector2 KnockbackSource;
        public BodyPart BodyPart;
        public List<CharacterModifier> Modifiers;

        public BodyHitResult(
            bool partBreak,
            bool weakBreak,
            float damage,
            int partDamage,
            float kbMod,
            Vector2 kbSource,
            BodyPart part,
            List<CharacterModifier> modifiers = null
            ) 
        {
            PartBreak = partBreak;
            WeakpointBreak = weakBreak;
            Damage = damage;
            PartDamage = partDamage;
            KnockbackMod = kbMod;
            KnockbackSource = kbSource;
            BodyPart = part;
            Modifiers = modifiers;
        }
    }

    public delegate void OnBodyHit(BodyHitResult hit);
    // A set of npc bodypart behaviours
    [Serializable]
    public class BodyManager
    {
        [SerializeField] List<BodyPart> m_bodyParts = default;
        public event OnBodyHit OnBodyPartHit;
        
        public virtual void Init() 
        { 
            foreach(BodyPart part in m_bodyParts) 
            {
                part.OnBodyHit += BodyPartHit;
            }
        }

        public virtual void Shutdown() 
        {
            foreach (BodyPart part in m_bodyParts)
            {
                part.OnBodyHit -= BodyPartHit;
            }

            OnBodyPartHit = null;
        }
        
        public virtual void ShowAllWeakpoint()
        {
            foreach(BodyPart weakPoint in m_bodyParts) 
            {
                weakPoint.ShowWeakpoint();
            }
        }

        public virtual void HideAllWeakpoint()
        {
            foreach (BodyPart weakPoint in m_bodyParts)
            {
                weakPoint.HideWeakpoint();
            }
        }

        public virtual void ShowWeakpoint(BodyPart part) 
        {
            if(part== null) return;

            if (m_bodyParts.Contains(part)) 
            {
                part.ShowWeakpoint();
            }
        }

        public virtual void HideWeakpoint(BodyPart part) 
        {
            if (part == null) return;

            if (m_bodyParts.Contains(part))
            {
                part.HideWeakpoint();
            }
        }

        protected void BodyPartHit(BodyHitResult hit) 
        {
            OnBodyPartHit?.Invoke(hit);
        }
    }
}
