using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Skill
{
    [Serializable]
    public class TraceInventory : MonoBehaviour
    {
        [SerializeField] protected int m_equippedTraceIndex = 0;
        [SerializeField] protected List<TraceAsset> m_traceList = default;

        public List<TraceAsset> TraceList { get { return m_traceList; } }

        public int EquippedTraceIndex { 
            get { return m_equippedTraceIndex; } 
            set { m_equippedTraceIndex = Mathf.Clamp(value, 0, m_traceList.Count - 1); } }

        public TraceAsset EquippedTrace 
        { get 
            { 
                return m_traceList[m_equippedTraceIndex]; 
            } 
        }

        public TraceAsset GetTrace(int index)
        {
            if (index >= m_traceList.Count || index < 0) 
            {
                return null;
            }

            return m_traceList[index];
        }

        public void AddTrace(TraceAsset trace) 
        {
            m_traceList.Add(trace);
        }



    }
}
