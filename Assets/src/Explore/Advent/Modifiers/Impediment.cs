using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    // A modifier to decrease movement range 
    public class Impediment : TacticalModifier, IStatusAilment, IMovementElement<TacticalStats>
    {
        [SerializeField] int m_numBeforeExpiry = default;
        [SerializeField] int m_rangeDecrease = default;
        public override ModifierContent Content => new ModifierContent
        {
            Name = m_resource.Content.Name,
            Description = $"{m_resource.Content.Description} by {m_rangeDecrease}. ({m_numBeforeExpiry} times left)",
            Icon = m_resource.Content.Icon
        };
        public Impediment(Impediment source): base(source)
        {
            m_numBeforeExpiry = source.m_numBeforeExpiry;
            m_rangeDecrease = source.m_rangeDecrease;
        }
        public Impediment() 
        {
            m_numBeforeExpiry = 1;
            m_rangeDecrease = 1;
        }
        public void Inflict(ICharacter target)
        {
            if (target is IModifiable modifiable) 
            {
                modifiable.ApplyModifier(this, Vfx, VfxTimeline);
            }
        }
        public virtual void OnCharacterMoved(TacticalStats stat)
        {
            m_numBeforeExpiry--;
            if( m_numBeforeExpiry <= 0) 
            {
                Expire();
            }
        }
        protected override TacticalStats Process_Internal(TacticalStats baseVal)
        {
            baseVal.MoveRange -= m_rangeDecrease;
            return baseVal;
        }
    }
}
