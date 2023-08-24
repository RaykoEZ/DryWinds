using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    
    public class Enhancement : TacticalModifier, IAttackModifier
    {
        [SerializeField] private int damageUp = default;
        public int DamageUp => damageUp;

        public Enhancement(Enhancement copy) : base(copy)
        {
            damageUp = copy.DamageUp;
        }


        public int Apply(int hitVal)
        {
            Expire();
            return hitVal + DamageUp;
        }
        protected override TacticalStats Process_Internal(TacticalStats baseVal)
        {
            return baseVal;
        }
    }
}
