using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnAdventurerDetected(Adventurer character);
    public class AdventurerDetector : MonoBehaviour
    {
        public event OnAdventurerDetected OnDetected;
        public event OnAdventurerDetected OnExitDetection;

        protected virtual void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.TryGetComponent(out Adventurer character))
            {
                OnDetected?.Invoke(character);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D c)
        {
            if (c.gameObject.TryGetComponent(out Adventurer character))
            {
                OnExitDetection?.Invoke(character);
            }
        }
    }
}
