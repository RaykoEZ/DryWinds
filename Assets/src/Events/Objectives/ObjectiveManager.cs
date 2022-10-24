using System.Collections.Generic;
using Curry.UI;
using UnityEngine;
using Curry.Events;

namespace Curry.Game
{
    [RequireComponent(typeof(DialogueTrigger))]
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField] protected List<GameObjective> m_objectives = default;
        protected List<IObjective> m_completedObjectives = new List<IObjective>();
        protected List<IObjective> m_failedObjectives = new List<IObjective>();

        public event OnObjectiveComplete ObjectiveCompleted;
        public event OnObjectiveComplete ObjectiveFail;

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
            CleanupObjective(completed);
            m_completedObjectives.Add(completed);
            ObjectiveCompleted?.Invoke(completed);
            // Do some animation/notification:
        }

        protected virtual void OnObjectiveFail(IObjective failed)
        {
            CleanupObjective(failed);
            m_failedObjectives.Add(failed);
            ObjectiveFail?.Invoke(failed);
            // Do some animation/notification:
        }

        void CleanupObjective(IObjective completed)
        {
            completed.OnComplete -= OnObjectiveComplete;
            completed.OnFail -= OnObjectiveFail;
            completed?.Shutdown();
            m_objectives.Remove(completed as GameObjective);
        }

        protected void PrepareObjective(GameObjective objective)
        {
            objective?.Init();
            objective.OnComplete += OnObjectiveComplete;
        }
        protected void ShutdownObjective(GameObjective objective)
        {
            objective?.Shutdown();
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
