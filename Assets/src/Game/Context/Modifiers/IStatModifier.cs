using Curry.Explore;
using System.Collections.Generic;

namespace Curry.Game
{
    public interface IStatModifier<T>
    {
        ModifierContent Content { get; }
        event OnModifierExpire<T> OnExpire;
        // Used to notify when modifier applied
        event OnModifierTrigger<T> OnTrigger;
        T Apply(T baseVal);
    }
    public interface IModifierContainer<T>
    {
        event OnModifierExpire<T> OnModExpire;
        event OnModifierTrigger<T> OnModTrigger;
        T Current { get; }
        IReadOnlyList<IStatModifier<T>> Modifiers { get; }
        void ApplyModifier(IStatModifier<T> mod);
        IReadOnlyList<ModifierContent> GetCurrentModifierDetails();

    }
}
