using Curry.UI;

namespace Curry.Game
{
    public interface ITalkable 
    {
        Dialogue Dialogues { get; }
        DialogueTrigger Trigger { get; }
    }
}
