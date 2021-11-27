using System.Collections.Generic;
using Curry.UI;
using UnityEngine;
using Curry.Events;

namespace Curry.Game
{
    [RequireComponent(typeof(DialogueTrigger))]
    public class ObjectiveManager : MonoBehaviour, ITalkable
    {
        [SerializeField] GameEventManager m_eventManager = default;
        [SerializeField] DialogueTrigger m_dialogueTrigger = default;
        [SerializeField] Dialogue m_dialogue = default;

        protected ObjectiveContainer m_objectives = new ObjectiveContainer();
        public Dialogue Dialogues { get { return m_dialogue; } }
        public DialogueTrigger Trigger { get { return m_dialogueTrigger; } }

        protected void OnEnable()
        {
            Init();
        }

        protected void OnDisable()
        {
            Shutdown();
        }

        public void OnObjectiveAchieve() 
        {
            m_dialogueTrigger.TriggerDialogue(m_dialogue, true);
        }

        protected virtual void Init() 
        { 
        
        }

        protected virtual void Shutdown()
        {

        }
    }

}
