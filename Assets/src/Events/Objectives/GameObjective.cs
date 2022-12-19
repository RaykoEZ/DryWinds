using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Events
{
    [Serializable]
    public abstract class GameObjective : MonoBehaviour, IObjective
    {
        public event OnObjectiveComplete OnComplete;
        public event OnObjectiveFail OnFail;

        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract void Init();
        public abstract void Shutdown();
        protected virtual void OnCompleteCallback() 
        {
            OnComplete?.Invoke(this);
        }
        protected virtual void OnFailCallback()
        {
            OnFail?.Invoke(this);
        }
    }
}
