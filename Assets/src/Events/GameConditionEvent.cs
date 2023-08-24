using Curry.Events;
using Curry.Util;
namespace Curry.Explore
{
    public class GameConditionEvent : EventInfo 
    { 
        public GameConditionAttribute ConditionsFulfilled { get; protected set; }
        public GameConditionEvent(GameConditionAttribute conditions) 
        {
            ConditionsFulfilled = conditions;
        }
    }
}
