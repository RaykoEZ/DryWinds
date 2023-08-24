using UnityEngine;

namespace Curry.Explore
{
    // A script that need to disable/enable player input
    // This needs to be contained in a Player Input interrupt container 
    public abstract class SceneInterruptBehaviour : MonoBehaviour 
    {
        public delegate void OnInputInterrupt();
        public event OnInputInterrupt OnInterruptBegin;
        public event OnInputInterrupt OnInterruptEnd;
        protected void StartInterrupt() 
        {
            OnInterruptBegin?.Invoke();
        }
        protected void EndInterrupt() 
        {
            OnInterruptEnd?.Invoke();
        }
    }
}
