using System.Collections;
namespace Curry.Explore
{
    public interface IHandEffect
    {
        IEnumerator HandEffect(GameStateContext c);
        IEnumerator OnLeaveHand(GameStateContext c);
    }
}
