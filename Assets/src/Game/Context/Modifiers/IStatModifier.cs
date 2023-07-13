using Curry.Explore;
using System;
using System.Collections.Generic;

namespace Curry.Game
{
    public delegate void OnStatUpdate<T>(T newStat);
    public interface IStatModifier<T> : IEquatable<IStatModifier<T>>
    {
        ModifierContent Content { get; }
        event OnModifierExpire<T> OnExpire;
        // Used to notify when modifier applied
        event OnModifierTrigger<T> OnTrigger;
        T Process(T baseVal);
    }
    public interface IModifierContainer<T>
    {
        event OnModifierExpire<T> OnModExpire;
        event OnModifierTrigger<T> OnModTrigger;
        event OnStatUpdate<T> OnStatUpdated;
        T Current { get; }
        IReadOnlyList<IStatModifier<T>> Modifiers { get; }
        void Refresh();
        void ApplyModifier(IStatModifier<T> mod);
        void RemoveModifier(IStatModifier<T> modRef);
        IReadOnlyList<ModifierContent> GetCurrentModifierDetails();
    }
}
