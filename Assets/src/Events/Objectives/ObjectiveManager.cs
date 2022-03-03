using System.Collections.Generic;
using Curry.UI;
using UnityEngine;
using Curry.Events;

namespace Curry.Game
{
    [RequireComponent(typeof(DialogueTrigger))]
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField] protected GameEventManager m_eventManager = default;
        [SerializeField] protected List<GameObjective> m_objectives = default;
        protected List<IObjective> m_completedObjectives = new List<IObjective>();
        public event OnObjectiveComplete ObjectiveCompleted;

        public IReadOnlyList<IObjective> ActiveObjectives { get { return m_objectives; } }
        public IReadOnlyList<IObjective> CompletedObjectives { get { return m_completedObjectives; } }

        protected void OnEnable()
        {
            Init();
        }

        protected void OnDisable()
        {
            Shutdown();
        }

        protected virtual void Init() 
        { 
            foreach(GameObjective objective in m_objectives)
            {
                PrepareObjective(objective);
            }
        }

        protected virtual void Shutdown()
        {
            foreach (GameObjective objective in m_objectives)
            {
                ShutdownObjective(objective);
            }
        }

        protected virtual void OnObjectiveComplete(IObjective completed) 
        {
            completed.OnComplete -= OnObjectiveComplete;
            completed?.Shutdown(m_eventManager);
            m_completedObjectives.Add(completed);
            m_objectives.Remove(completed as GameObjective);
            // Do some animation/notification:
            ObjectiveCompleted?.Invoke(completed);
        }
        protected void PrepareObjective(GameObjective objective)
        {
            objective?.Init(m_eventManager);
            objective.OnComplete += OnObjectiveComplete;
        }
        protected void ShutdownObjective(GameObjective objective)
        {
            objective?.Shutdown(m_eventManager);
            objective.OnComplete -= OnObjectiveComplete;
        }

        public void AddObjective(IObjective objective) 
        {
            GameObjective obj = objective as GameObjective;
            PrepareObjective(obj);
            m_objectives.Add(obj);
        }
    }

}
