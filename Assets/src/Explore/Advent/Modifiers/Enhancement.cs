using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Enhancement : TacticalModifier, IAttackModifier
    {
        [SerializeField] protected int m_damageUp = default;
        public int Apply(int hitVal)
        {
            return hitVal + m_damageUp;
        }
        protected override TacticalStats Process_Internal(TacticalStats baseVal)
        {
            return baseVal;
        }
    }
}
