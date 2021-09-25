using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;

namespace Curry.Game
{
    public interface ISpawner
    {
        GameObject AssetRef { get; }
        GameObject Spawn(Transform parent = null);
    }

    [Serializable]
    public struct SpawnerProperties 
    {
        [SerializeField] public bool AutoSpawn;
        [SerializeField] public float AutoSpawnTimeInterval;
        [SerializeField] public int AutoSpawnAmountCap;
        [SerializeField] public Transform DefaultParent;

    }

    public class NpcSpawner : MonoBehaviour, ISpawner
    {
        // Reference to the npc asset
        [SerializeField] protected PrefabLoader m_npcRef = default;
        [SerializeField] protected SpawnerProperties m_spawnProperties = default;
        protected float m_spawnTimer = 0f;

        protected List<GameObject> m_npcObjs = new List<GameObject>();
        public GameObject AssetRef { get; protected set; }
        
        protected virtual void Start() 
        {
            m_npcRef.OnLoadSuccess += (obj) => { AssetRef = obj; };
            m_npcRef.LoadAsset();
        }
        
        protected virtual void Update() 
        {    
            if (m_spawnProperties.AutoSpawn && m_npcObjs.Count < m_spawnProperties.AutoSpawnAmountCap) 
            {
                m_spawnTimer += Time.deltaTime;
                if (m_spawnTimer >= m_spawnProperties.AutoSpawnTimeInterval && AssetRef != null) 
                {
                    Spawn();
                    m_spawnTimer = 0f;
                }
            }
        }

        public virtual GameObject Spawn(Transform parent = null)
        {
            if (AssetRef == null) 
            { 
                return null; 
            }

            Transform transform = parent == null ? m_spawnProperties.DefaultParent : parent;
            GameObject obj = Instantiate(AssetRef, transform);

            m_npcObjs.Add(obj);

            return obj;
        }
    }

}

