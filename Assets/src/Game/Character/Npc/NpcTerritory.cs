using Curry.Game;
using Curry.Skill;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Ai
{
    [AddComponentMenu("Curry/Ai/Territory")]
    public class NpcTerritory : MonoBehaviour
    {
        [SerializeField] protected Collider2D m_collider = default;
        public virtual Bounds Boundary { get { return m_collider.bounds; } }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            BaseNpc npc = collision.gameObject?.GetComponent<BaseNpc>();
            if(npc != null) 
            {
                npc.Retreat();
            }
        }
    }
}
