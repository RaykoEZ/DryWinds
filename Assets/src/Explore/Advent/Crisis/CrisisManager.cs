using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
using Curry.Game;

namespace Curry.Explore
{
    [Serializable]
    public class CrisisPoolCollection : PoolCollection<CrisisBehaviour>
    {
    }

    [Serializable]
    public class CrisisInstanceManager : InstanceManager<CrisisBehaviour>
    {
        [SerializeField] protected CrisisPoolCollection m_pool = default;
        protected override PoolCollection<CrisisBehaviour> Pool { get { return m_pool; } }
    }

    public class CrisisManager : MonoBehaviour
    {
        [SerializeField] protected CurryGameEventListener m_onCreateCrisis = default;
        [SerializeField] protected Transform m_parent = default;
        [SerializeField] protected Tilemap m_tileMap = default;
        [SerializeField] protected CrisisInstanceManager m_instanceManager = default;

        List<CrisisBehaviour> m_crisisList = new List<CrisisBehaviour>();
        // Use this for initialization
        void OnEnable()
        {
            m_onCreateCrisis?.Init();
        }

        void OnDisable()
        {
            m_onCreateCrisis?.Shutdown();
        }

        public void CreateCrisis<T>(CrisisBehaviour crisis, T property, Vector3Int startPos) where T : CrisisProperty
        {
            if(crisis == null) 
            { 
                return; 
            }
            // Instanciate and pool new crisis instace
        }
    }
}