namespace Curry.Game
{
    // Entities that prompt player when player is near its vicinity
    public interface IProximityPrompt
    {
        void OnCloseBy(Player player);
    }
}
