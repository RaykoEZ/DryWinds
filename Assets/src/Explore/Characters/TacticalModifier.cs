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
            return Apply_Internal(baseVal);
        }
        protected void Trigger()
        {
            OnTrigger?.Invoke(this);
        }
        protected void Expire()
        {
            OnExpire?.Invoke(this);
        }
        protected abstract TacticalStats Apply_Internal(TacticalStats baseVal);
    }
}
