using Curry.Events;

namespace Curry.Explore
{
    public class CharacterInfo : EventInfo
    {
        public ICharacter Character { get; protected set; }
        public CharacterInfo(ICharacter stats) 
        {
            Character = stats;
        }
    }
}
