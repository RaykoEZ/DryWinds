using UnityEngine;
using Curry.Game;
using Curry.Explore;

namespace Curry.Ai
{
    public delegate void OnCharacterDetected(BaseCharacter character);

    [RequireComponent(typeof(Collider2D))]
    public class CharacterDetector : MonoBehaviour
    {
        public event OnCharacterDetected OnDetected;
        public event OnCharacterDetected OnExitDetection;

        protected virtual void OnTriggerEnter2D(Collider2D c)
        {
            if(c.gameObject.TryGetComponent(out BaseCharacter character)) 
            {
                OnDetected?.Invoke(character);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D c)
        {
            if (c.gameObject.TryGetComponent(out BaseCharacter character))
            {
                OnExitDetection?.Invoke(character);
            }
        }
    }

}
