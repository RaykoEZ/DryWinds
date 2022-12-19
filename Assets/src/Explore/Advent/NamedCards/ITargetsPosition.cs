using UnityEngine;
namespace Curry.Explore
{
    public interface ITargetsPosition
    { 
        public int Range { get; }
        public Vector3 Target { get; }
        public bool Satisfied { get; }
        public void SetTarget(Vector3 target);
    }
}
