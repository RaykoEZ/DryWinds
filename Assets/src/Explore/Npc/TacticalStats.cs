using System;

namespace Curry.Explore
{
    [Serializable]
    public enum TacticalVisibility
    {
        Visible,
        Hidden
    }
    [Serializable]
    public struct TacticalStats 
    {
        public int ScoutRange;
        public int Speed;
        public int AttackCountdown;
        public TacticalVisibility Visibility;
    }

}
