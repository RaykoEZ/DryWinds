using System;

namespace Curry.Explore
{
    public interface IEnemy 
    {
        TacticalStats InitStatus { get; }
        TacticalStats CurrentStatus { get; }
        void Reveal();
        void Hide();
        void TakeHit();
        void Affect(Func<TacticalStats,TacticalStats> effect);
    }

}
