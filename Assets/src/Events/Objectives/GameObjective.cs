using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Events
{
    [Serializable]
    public abstract class GameObjective : MonoBehaviour, IObjective, IEquatable<GameObjective>
    {
        public event OnObjectiveComplete OnComplete;
        public event OnObjectiveFail OnFail;

        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract ICondition<IComparable> ObjectiveCondition { get; }

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
        public static bool operator ==(GameObjective t1, GameObjective t2)
        {
            return t1.ObjectiveCondition == t2.ObjectiveCondition;
        }

        public static bool operator !=(GameObjective t1, GameObjective t2)
        {
            return t1.ObjectiveCondition != t2.ObjectiveCondition;
        }

        public override bool Equals(object obj)
        {
            if (obj is GameObjective objective)
            {
                return Equals(objective);
            }
            return false;
        }

        public bool Equals(GameObjective other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return ObjectiveCondition.GetHashCode();
        }
    }
}
