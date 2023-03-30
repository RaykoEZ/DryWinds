using Curry.Explore;
using System.Collections.Generic;

namespace Curry.Game
{
    public interface IStatModifier<T>
    { 
        string Name { get; }
        event OnModifierExpire<T> OnModifierExpire;
        // Used to notify when modifier applied
        event OnModifierTrigger<T> OnTrigger;
        T Apply(T baseVal);
        T Revert(T baseVal);
    }
    public interface IModifierContainer<T>
    {
        event OnModifierExpire<T> OnModExpire;
        event OnModifierTrigger<T> OnModTrigger;
        T Result { get; }
        IReadOnlyList<IStatModifier<T>> Modifiers { get; }
        void AddModifier(IStatModifier<T> mod);
    }
}
