namespace Curry.Game
{
    public interface IStatModifier<T>
    { 
        string Name { get; }
        event OnModifierExpire OnModifierExpire;
        // Used to apply additional modifiers from a modifier's effect
        event OnModifierChain OnModifierChain;
        // Used to notify when modifier applied
        event OnModifierTrigger OnTrigger;
        T Apply(T baseVal);
        T Revert(T baseVal);
    }
}
