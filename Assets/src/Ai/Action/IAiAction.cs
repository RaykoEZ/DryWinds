
namespace Curry.Ai
{
    public delegate void OnActionFinish();
    public interface IAiAction
    {
        event OnActionFinish OnFinish;
        bool ActionInProgress { get; }

        bool PreCondition(NpcWorldState args);
        bool ExitCondition(NpcWorldState args);
    }
}
