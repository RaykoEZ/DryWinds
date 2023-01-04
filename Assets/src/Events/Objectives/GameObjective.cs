using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Events
{
    [Serializable]
    public abstract class GameObjective : PoolableBehaviour, IObjective
    {
        public event OnObjectiveComplete OnComplete;
        public event OnObjectiveFail OnFail;

        public abstract string Title { get; }
        public abstract string Description { get; }
        public virtual void Init() { }
        public virtual void Shutdown() { }
        public override void Prepare() 
        {
            Init();
        }
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
