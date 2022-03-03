namespace Curry.Game
{
    // Entities that prompt player when player is near its vicinity
    public interface IProximityPrompt
    {
        EntityProperty Property { get; }
        void OnCloseBy(Player player);
    }
}
