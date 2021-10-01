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

    public class InteractableSpawner : MonoBehaviour, ISpawner
    {
        // Reference to the npc asset
        [SerializeField] protected PrefabLoader m_objRef = default;
        [SerializeField] protected SpawnerProperties m_spawnProperties = default;
        [SerializeField] protected InteractableInstanceManager m_instanceManager = default;
        protected float m_spawnTimer = 0f;
        protected int m_spawnCounter = 0;
        public GameObject AssetRef { get; protected set; }
        
        protected virtual void Start() 
        {
            m_objRef.OnLoadSuccess += (obj) => 
            { 
                AssetRef = obj;
                m_instanceManager.PrepareNewInstance(AssetRef, m_spawnProperties.DefaultParent);
            };
            m_objRef.LoadAsset();
        }
        
        protected virtual void Update() 
        {
            AutoSpawn();
        }

        protected virtual void AutoSpawn() 
        {
            if (m_spawnProperties.AutoSpawn && m_spawnCounter < m_spawnProperties.AutoSpawnAmountCap)
            {
                m_spawnTimer += Time.deltaTime;
                if (m_spawnTimer >= m_spawnProperties.AutoSpawnTimeInterval && AssetRef != null)
                {
                    Spawn(m_spawnProperties.DefaultParent);
                    ++m_spawnCounter;
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
            GameObject obj = m_instanceManager.GetInstanceFromCurrentPool().gameObject;
            obj.transform.SetParent(transform);
            return obj;
        }
    }

}

