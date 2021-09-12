using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public delegate void OnCharacterDetected(BaseCharacter character);

    [RequireComponent(typeof(Collider2D))]
    public class DetectionHandler : MonoBehaviour
    {
        [SerializeField] Collider2D m_range = default;
        [SerializeField] protected float m_reactionTime = default;

        public event OnCharacterDetected OnDetected;
        public event OnCharacterDetected OnExitDetection;

        protected virtual void OnTriggerEnter2D(Collider2D c) 
        {
            if(c.gameObject.TryGetComponent(out BaseCharacter character)) 
            {
                StartCoroutine(ReactToDetection(character));
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D c)
        {
            if (c.gameObject.TryGetComponent(out BaseCharacter character))
            {
                OnExitDetection?.Invoke(character);
            }
        }

        protected virtual IEnumerator ReactToDetection(BaseCharacter character) 
        {
            yield return new WaitForSeconds(m_reactionTime);
            // if character is still in range of view after some time, detect target.
            if (m_range.bounds.Contains(character.transform.position)) 
            {
                OnDetected?.Invoke(character);
            }
        }
    }
}
