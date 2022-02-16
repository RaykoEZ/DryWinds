namespace Curry.Skill
{
    public interface ITimeLimit
    { 
        float Duration { get; }
        float TimeElapsed { get; }
        void OnDefeat();
    }
}
