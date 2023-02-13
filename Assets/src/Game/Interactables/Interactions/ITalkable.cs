using Curry.UI;

namespace Curry.Game
{
    public interface ITalkable 
    {
        EncounterResultAttribute Dialogues { get; }
        DialogueTrigger Trigger { get; }
    }
}
