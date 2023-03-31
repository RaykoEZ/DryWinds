using Curry.Game;
using UnityEngine;

namespace Curry.Explore
{
    public abstract class TacticalModifier : IStatModifier<TacticalStats>
    {
        // Name of the modifier, e.g. a skill/item name
        [SerializeField] protected string m_name;
        public string Name => m_name;
        public event OnModifierExpire<TacticalStats> OnExpire;
        public event OnModifierTrigger<TacticalStats> OnTrigger;
        public TacticalStats Apply(TacticalStats baseVal) 
        {
            OnTrigger?.Invoke(this);
            return Apply_Internal(baseVal);
        }
        public TacticalStats Expire(TacticalStats baseVal) 
        {
            OnExpire?.Invoke(this);
            return Revert_Internal(baseVal);
        }

        protected abstract TacticalStats Apply_Internal(TacticalStats baseVal);
        protected abstract TacticalStats Revert_Internal(TacticalStats baseVal);
    }
}
