using Curry.Util;
using UnityEngine;
namespace Curry.Explore
{
    public interface ITargetsPosition : IActivationCondition<Vector3>
    { 
        public RangeMap TargetingRange { get; }
    }

    public interface IActivationCondition<T>
    {
        public bool ConditionsSatisfied { get; }
        public void SetTarget(T target);
    }
}
