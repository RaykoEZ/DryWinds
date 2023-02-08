using Curry.UI;

namespace Curry.Game
{
    public interface ITalkable 
    {
        EncounterResult Dialogues { get; }
        DialogueTrigger Trigger { get; }
    }
}
