using Curry.UI;
using UnityEngine;

namespace Curry.Game
{
    [RequireComponent(typeof(DialogueTrigger))]
    public class ObjectiveManager : MonoBehaviour, ITalkable
    {
        [SerializeField] DialogueTrigger m_dialogueTrigger = default;
        [SerializeField] Dialogue m_dialogue = default;

        public Dialogue Dialogues { get { return m_dialogue; } }
        public DialogueTrigger Trigger { get { return m_dialogueTrigger; } }

        public void OnObjectiveAchieve() 
        {
            m_dialogueTrigger.TriggerDialogue(m_dialogue, true);
        }
    }
}
