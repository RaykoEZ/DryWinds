using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Events
{
    [Serializable]
    public abstract class GameObjective : MonoBehaviour, IObjective, IEquatable<GameObjective>
    {
        public event OnObjectiveComplete OnComplete;
        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract ICondition<IComparable> ObjectiveCondition { get; }

        public abstract void Init(GameEventManager eventManager);
        public abstract void Shutdown(GameEventManager eventManager);
        protected virtual void OnCompleteCallback() 
        {
            OnComplete?.Invoke(this);
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

    [Serializable]
    public class ResueObjective : GameObjective
    {
        [SerializeField] AmountAchieved m_condition = default;
        public override string Title { get { return "VIP Rescue"; } }

        public override string Description { get { return "Rescue VIP within time limit: " + m_condition.Description; } }

        public override ICondition<IComparable> ObjectiveCondition { get { return m_condition as ICondition<IComparable>; } }

        public override void Init(GameEventManager eventManager)
        {
            throw new NotImplementedException();
        }

        public override void Shutdown(GameEventManager eventManager)
        {
            throw new NotImplementedException();
        }
    }
}
