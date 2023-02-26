using UnityEngine;
namespace Curry.Explore
{
    public interface ITargetsPosition : IActivationCondition<Vector3>
    { 
        public int Range { get; }
    }

    public interface IActivationCondition<T>
    {
        public bool Satisfied { get; }
        public T Target { get; }
        public void SetTarget(T target);
    }
}
