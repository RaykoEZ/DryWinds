using System.Collections;

namespace Curry.Explore
{
    public interface IEndOfTurnEffect
    {
        IEnumerator OnEndOfTurn(GameStateContext c);
    }
}
