using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Curry.Game;

namespace Curry.Skill
{
    public class BaseTracerBrush : MonoBehaviour
    {
        [SerializeField] PoolManager m_poolManager = default;

        protected BaseTrace m_currentTracer = default;
        protected string m_currentTraceId = default;
        protected ObjectPool m_currentPool = default;
        protected bool m_equipingTrace = false;
        protected Vector2 m_previousMousePos = default;

        TraceStats m_currentTraceStat = default;

        public void EquipTrace(TraceAsset trace) 
        {
            m_equipingTrace = true;
            m_currentTraceId = trace.name;
            m_currentTraceStat = trace.TraceStats;
            if (m_poolManager.ContainsPool(m_currentTraceId)) 
            {
                m_currentPool = m_poolManager.GetPool(m_currentTraceId);
            }
            else 
            {
                m_currentPool = m_poolManager.AddPool(m_currentTraceId, trace.Prefab);
            }
            m_equipingTrace = false;
        }

        public virtual void OnTraceEnd() 
        {
            m_currentTracer = null;
        }

        public virtual void Draw(CharacterStats stats)
        {
            // don't draw when we are switching tracers
            if (m_equipingTrace) 
            {
                return;
            }

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint
                (Mouse.current.position.ReadValue());

            if (mousePosition != m_previousMousePos)
            {
                float length = m_previousMousePos == null ? 0f : Vector2.Distance(mousePosition, m_previousMousePos);
                float SPCost = m_currentTraceStat.SpCostScale * length;
                if (stats.SP >= SPCost)
                {
                    BeginDraw(mousePosition, length);
                    stats.SP -= SPCost;
                }
                else
                {
                    OnTraceEnd();
                }
            }
            //update mousePos log
            m_previousMousePos = mousePosition;
        }

        void BeginDraw(Vector2 mousePos, float length)
        {
            // start a new stroke if we hold LMB and is moving
            if (m_currentTracer == null)
            {
                // make new stroke, can be pooled objects in the future
                GameObject newStroke = m_currentPool?.GetItem();
                m_currentTracer = newStroke.GetComponent<BaseTrace>();
            }

            m_currentTracer.OnDraw(mousePos, length);
            m_currentTracer.gameObject.SetActive(true);
        }

    }
}
