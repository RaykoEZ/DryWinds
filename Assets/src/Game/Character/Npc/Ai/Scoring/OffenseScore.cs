namespace Curry.Game
{
    public class OffenseScore : IScorable<OffenseArgs>
    {
        public float Evaluate(OffenseArgs priorityVal) 
        { 
            return 1f; 
        }
    }
}
