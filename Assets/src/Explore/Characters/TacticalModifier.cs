using Curry.Game;
using UnityEngine;

namespace Curry.Explore
{
    public abstract class TacticalModifier : IStatModifier<TacticalStats>
    {
        // Name of the modifier, e.g. a skill/item name
        [SerializeField] protected string m_name;
        public string Name => m_name;
        public event OnModifierExpire<TacticalStats> OnModifierExpire;
        public event OnModifierTrigger<TacticalStats> OnTrigger;
        public abstract TacticalStats Apply(TacticalStats baseVal);
        public abstract TacticalStats Revert(TacticalStats baseVal);
    }
}
