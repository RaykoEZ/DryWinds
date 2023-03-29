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
}
