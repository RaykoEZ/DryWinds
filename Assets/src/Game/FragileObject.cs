using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class FragileObject : Interactable
    {
        [SerializeField] int m_durability = default;
        [SerializeField] float m_iFrameLength = default;

        bool m_isInvincible = false;
        protected int m_currentDurability = 1;
        
        protected virtual void Start() 
        {
            m_currentDurability = m_durability;     
        }

        protected override void OnClash(Collision2D collision)
        {
            Interactable incomingInterable = collision.gameObject.GetComponent<Interactable>();

            if (incomingInterable != null)
            {
                OnTakeDamage(0f);
            }
        }

        public override void OnTakeDamage(float damage)
        {
            if (m_isInvincible) { return; }
            StartCoroutine(IFrameCounter());
            m_currentDurability--;
            if (m_currentDurability <= 0)
            {
                OnDefeat();
            }
        }

        public override void OnDefeat()
        {
            // Should return to a pool and play a disappearing animation.
            Destroy(gameObject);
        }

        IEnumerator IFrameCounter() 
        {
            m_isInvincible = true;
            yield return new WaitForSeconds(m_iFrameLength);
            m_isInvincible = false;
        }
    }
}
