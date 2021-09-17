namespace Curry.Game
{
    public class SurvivalScore : IScorable<SurvivalArgs>
    {
        public float Evaluate(SurvivalArgs priorityVal) 
        { 
            return 1f; 
        }


    }
}
