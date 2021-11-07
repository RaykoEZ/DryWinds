namespace Curry.Game
{
    public interface IActionProperty
    {
        string Name { get; }
        float SpCost { get; }
        float CooldownTime { get; }
        float MaxWindupTime { get; }

    }
}
