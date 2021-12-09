using System.Collections.Generic;
using Curry.UI;
using UnityEngine;
using Curry.Events;

namespace Curry.Game
{
    [RequireComponent(typeof(DialogueTrigger))]
    public class ObjectiveManager : MonoBehaviour, ITalkable
    {
        [SerializeField] protected GameEventManager m_eventManager = default;
        [SerializeField] protected List<GameObjective> m_objectives = default;
        [SerializeField] protected DialogueTrigger m_dialogueTrigger = default;
        [SerializeField] protected Dialogue m_dialogue = default;
        public Dialogue Dialogues { get { return m_dialogue; } }
        public DialogueTrigger Trigger { get { return m_dialogueTrigger; } }
        public IReadOnlyList<IObjective> Objectives { get { return m_objectives; } }

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
