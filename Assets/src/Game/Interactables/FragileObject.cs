using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class FragileObject : Interactable, IFragileObject
    {
        [SerializeField] int m_durability = default;

        protected int m_currentDurability = 1;

        protected virtual void Start()
        {
            Prepare();
        }

        public override void Prepare()
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
            m_currentDurability--;
            if (m_currentDurability <= 0)
            {
                UpdatePathfinder();
                OnDefeat();
            }
        }
    }
}
