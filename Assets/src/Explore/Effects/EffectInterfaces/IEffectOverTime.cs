using System.Collections;

namespace Curry.Explore
{
    public interface IEffectOverTime
    {
        int TimeInterval { get;}
        IEnumerator OnTick(GameStateContext c);
    }
}
