using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    // A modifier to decrease movement range 
    public class Impediment : TacticalModifier, IMovementElement<TacticalStats>, ITargetEffectModule
    {
        [SerializeField] int m_numBeforeExpiry = default;
        [SerializeField] int m_rangeDecrease = default;
        public override ModifierContent Content => new ModifierContent
        {
            Name = "Impediment",
            Description = $"Reduce Movement Range by {m_rangeDecrease}. ({m_numBeforeExpiry} times left)",
            Icon = m_modIcon
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
            m_modIcon = null;
        }
        public void ApplyEffect(ICharacter target)
        {
            if(target is IModifiable modifiable) 
            {
                modifiable.CurrentStats.ApplyModifier(this);
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
        protected override TacticalStats Apply_Internal(TacticalStats baseVal)
        {
            baseVal.MoveRange -= m_rangeDecrease;
            return baseVal;
        }
    }
}
