
namespace Curry.Ai
{
    public interface IAiGoal 
    {
        bool PreCondition(NpcWorldState args);
        float Priority(NpcWorldState args);
    }
}
