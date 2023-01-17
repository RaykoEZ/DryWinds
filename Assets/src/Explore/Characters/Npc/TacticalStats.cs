using System;

namespace Curry.Explore
{
    [Serializable]
    public enum ObjectVisibility
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
        public ObjectVisibility Visibility;
    }

}
