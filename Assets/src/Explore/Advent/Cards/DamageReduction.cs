using UnityEngine;

namespace Curry.Explore
{
    public class DamageReduction : TacticalModifier, IDamageModifier
    {
        [SerializeField] protected int m_damageReduction = 0;
        public int Value => m_damageReduction;
        public DamageReduction(int reduction) 
        {
            m_damageReduction = reduction;
        }
        public DamageReduction(DamageReduction copy) : base(copy) 
        {
            m_damageReduction = copy.m_damageReduction;
        }
        public int Apply(int hitVal)
        {
            return Mathf.Max(0, hitVal - m_damageReduction);
        }
        protected override TacticalStats Process_Internal(TacticalStats baseVal)
        {
            return baseVal;
        }
    }
}