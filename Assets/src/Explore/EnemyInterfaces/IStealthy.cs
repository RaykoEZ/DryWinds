namespace Curry.Explore
{
    // Stealth level is used to check whether a stealthy enemy is revealed when scanning tiles
    public interface IStealthy : IEnemy
    {
        int StealthLevel { get; }
    }
}
