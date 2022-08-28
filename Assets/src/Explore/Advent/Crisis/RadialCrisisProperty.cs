using Curry.Game;

namespace Curry.Explore
{
    public interface RadialCrisisProperty : CrisisProperty
    {
        float StartRadius { get;}
        // radius growth in uunit per sec
        float GrowthRate { get;}
    }
}
