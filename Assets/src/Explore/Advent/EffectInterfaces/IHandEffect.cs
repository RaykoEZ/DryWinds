namespace Curry.Explore
{
    public interface IHandEffect
    {
        void HandEffect(GameStateContext c);
        void OnLeaveHand(GameStateContext c);
    }
}
