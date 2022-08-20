using Curry.Game;

namespace Curry.Explore
{
    public interface RadialCrisisProperty<T> : CrisisProperty<T> where T : Interactable
    {
        float StartRadius { get;}
        // radius growth in uunit per sec
        float GrowthRate { get;}
    }
}
