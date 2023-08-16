using System.Collections.Generic;
using Curry.UI;
using UnityEngine;
using Curry.Events;

namespace Curry.Game
{

    public delegate void OnAllComplete();
    public class ObjectiveManager : MonoBehaviour
    {
        /// Preloaded objectives
        [SerializeField] protected List<GameObjective> m_criticalObjectives = default;
        [SerializeField] protected List<GameObjective> m_optionalObjectives = default;

        protected List<IObjective> m_critical = new List<IObjective>();
        protected List<IObjective> m_optional = new List<IObjective>();
        protected List<IObjective> m_completed = new List<IObjective>();
        protected List<IObjective> m_failed = new List<IObjective>();

        public event OnAllComplete OnCriticalComplete;
        public event OnObjectiveComplete ObjectiveCompleted;
        public event OnObjectiveFail OnFailure;
        public event OnObjectiveFail OnCriticalFailure;
        public IReadOnlyList<IObjective> CriticalObjectives { get { return m_critical; } }
        public IReadOnlyList<IObjective> OptionalObjectives { get { return m_optional; } }
        public IReadOnlyList<IObjective> CompletedObjectives { get { return m_completed; } }

        protected void Start()
        {
            Init();
        }

        protected void OnDestroy()
        {
            Shutdown();
        }

        protected virtual void Init() 
        {
            foreach (IObjective objective in m_criticalObjectives)
            {
                m_critical.Add(objective);
                PrepareObjective(objective);
            }
            foreach (IObjective objective in m_optionalObjectives)
            {
                m_optional.Add(objective);
                PrepareObjective(objective);
            }
        }

        protected virtual void Shutdown()
        {
            foreach (IObjective objective in CriticalObjectives)
            {
                ShutdownObjective(objective);
            }
            foreach (IObjective objective in OptionalObjectives)
            {
                ShutdownObjective(objective);
            }
        }
        public void AddObjective(IObjective objective, bool isCritical = false)
        {
            PrepareObjective(objective);
            if (isCritical) 
            {
                m_critical.Add(objective);
            }
            else 
            {
                m_optional.Add(objective);
            }
        }
        protected virtual void OnObjectiveComplete(IObjective completed) 
        {
            if (m_critical.Contains(completed))
            {
                OnCriticalComplete?.Invoke();
            }
            CleanupObjective(completed);
            m_completed.Add(completed);
            ObjectiveCompleted?.Invoke(completed);
            // Do some animation/notification:
        }

        protected virtual void OnObjectiveFail(IObjective failed)
        {
            if (m_critical.Contains(failed))
            {
                OnCriticalFailure?.Invoke(failed);
            }
            else 
            {
                OnFailure?.Invoke(failed);
            }
            CleanupObjective(failed);
            m_failed.Add(failed);

            // Do some animation/notification:
        }

        void CleanupObjective(IObjective completed)
        {
            completed.OnComplete -= OnObjectiveComplete;
            completed.OnFail -= OnObjectiveFail;
            completed?.Shutdown();
            m_optional.Remove(completed);
            m_critical.Remove(completed);
        }

        protected void PrepareObjective(IObjective objective)
        {
            objective?.Init();
            objective.OnComplete += OnObjectiveComplete;
            objective.OnFail += OnObjectiveFail;
        }
        protected void ShutdownObjective(IObjective objective)
        {
            objective?.Shutdown();
            objective.OnComplete -= OnObjectiveComplete;
            objective.OnFail -= OnObjectiveFail;
        }
    }

}
