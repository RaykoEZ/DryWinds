using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Events
{
    [Serializable]
    public abstract class GameObjective : MonoBehaviour, IObjective
    {
        public event OnObjectiveComplete OnComplete;
        public abstract ICondition<IComparable> ObjectiveCondition { get; }
        public abstract void Init(GameEventManager eventManager);
        public abstract void Shutdown(GameEventManager eventManager);
        protected void OnCompleteCallback() 
        {
            OnComplete?.Invoke();
        }
    }
}
