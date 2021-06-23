using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Skill
{
    [Serializable]
    public class TraceInventory : MonoBehaviour
    {
        [SerializeField] List<TraceAsset> m_traceList = default;
    
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
