using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class NpcSpawner : MonoBehaviour
    {
        [SerializeField] protected GameObject m_npcRef = default;
        [SerializeField] protected Transform m_parent = default;
        [SerializeField] protected float m_autoSpawnInterval = default;
        [SerializeField] protected int m_spawnCap = default;
        protected float m_spawnTimer = 0f;

        protected List<GameObject> m_npcObjs = new List<GameObject>();

        protected virtual void Update() 
        {    
            if (m_autoSpawnInterval > 0f && m_npcObjs.Count < m_spawnCap) 
            {
                m_spawnTimer += Time.deltaTime;
                if (m_spawnTimer >= m_autoSpawnInterval) 
                {
                    Spawn();
                    m_spawnTimer = 0f;
                }
            }
        
        }

        public virtual void Spawn()
        {
            GameObject obj = Instantiate(m_npcRef, m_parent);

            m_npcObjs.Add(obj);
        }
    }

}

