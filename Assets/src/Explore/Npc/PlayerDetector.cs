using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnPlayerDetected(IPlayer player);
    public class PlayerDetector : MonoBehaviour
    {
        public event OnPlayerDetected OnDetected;
        public event OnPlayerDetected OnExitDetection;

        public bool PlayerDetected { get; protected set; } = false;
        protected virtual void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.TryGetComponent(out IPlayer character))
            {
                PlayerDetected = true;
                OnDetected?.Invoke(character);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D c)
        {
            if (c.gameObject.TryGetComponent(out IPlayer character))
            {
                PlayerDetected = false;
                OnExitDetection?.Invoke(character);
            }
        }
    }
}
