using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class ArenaRing : Interactable
    {
        [SerializeField] float m_initScale = default;

        protected void Start()
        {
            SetRingScale(m_initScale);
        }

        public void SetRingScale(float scale) 
        {
            transform.localScale = new Vector3(scale, scale, scale);
            EdgeCollider2D col = (EdgeCollider2D)m_hurtBox;
            col.edgeRadius = 0.5f * scale;
        }
    }
}
