using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Curry.Explore
{
    [Serializable]
    public class CrisisCollection 
    {
        [SerializeField] List<CrisisResource> m_crisisList = default;
        List<Crisis> m_triggered = new List<Crisis>();
        Queue<Crisis> m_pending = new Queue<Crisis>();
        public List<CrisisResource> AllCrsis => m_crisisList;
        public List<Crisis> Triggered => m_triggered;
        public Queue<Crisis> Pending => m_pending;
        public void Init() 
        {
            m_triggered.Clear();
            m_pending.Clear();
            foreach (var item in m_crisisList)
            {
                m_pending.Enqueue(item.GetContent());
            }
        }
    }
    public class CrisisManager : SceneInterruptBehaviour
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_label = default;
        [SerializeField] TimeManager m_time = default;
        [SerializeField] Tilemap m_terrain = default;
        [SerializeField] GameStateManager m_gameState = default;
        [SerializeField] CrisisCollection m_collection = default;
        Crisis m_current;
        // Use this for initialization
        void Start()
        {
            m_time.OnOutOfTimeTrigger += OnCrisisTrigger;
            m_collection.Init();
        }
        // view incoming crisis and currently activated ones, need menu
        public void ViewCrisisState() 
        { 
        
        }
        void OnCrisisTrigger() 
        { 
            // Store old crisis in list of triggered items
            if (m_current != null) 
            {
                m_collection.Triggered.Add(m_current);
            }

            if (m_collection.Pending.Count > 0) 
            {
                m_current = m_collection.Pending.Dequeue();
                StartCoroutine(HandleCrisis());
            }
        }

        IEnumerator HandleCrisis() 
        {
            StartInterrupt();
            yield return m_current?.Activate(m_gameState.GetCurrent(), m_terrain);
            yield return new WaitForEndOfFrame();
            EndInterrupt();
        }
    }
}