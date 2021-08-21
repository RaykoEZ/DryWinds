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

        protected BaseTrace m_currentTracerBehaviour = default;
        protected string m_currentTraceId = default;
        protected ObjectPool m_currentPool = default;
        protected bool m_isDrawing = false;
        protected bool m_equipingTrace = false;
        protected Vector2 m_previousDrawPos = default;

        public void EquipTracer(TraceAsset trace) 
        {
            m_equipingTrace = true;
            m_currentTraceId = trace.name;
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
            m_isDrawing = false;
        }

        public virtual void Draw(CharacterStats stats, Vector2 drawTo)
        {
            // don't draw when we are switching tracers
            if (m_equipingTrace) 
            {
                return;
            }

            // start a new stroke if we hold LMB and is moving
            if (!m_isDrawing)
            {
                // make new stroke, can be pooled objects in the future
                GameObject newTraceBehaviour = m_currentPool?.GetItem();
                m_currentTracerBehaviour = newTraceBehaviour.GetComponent<BaseTrace>();
            }

            if (drawTo != m_previousDrawPos)
            {
                float length = m_previousDrawPos == null ? 0f : Vector2.Distance(drawTo, m_previousDrawPos);
                float SPCost = m_currentTracerBehaviour.SkillProperties.SpCost * length;
                if (stats.SP >= SPCost)
                {
                    m_isDrawing = true;
                    m_currentTracerBehaviour.OnDraw(drawTo, length);
                    m_currentTracerBehaviour.gameObject.SetActive(true);
                    stats.SP -= SPCost;
                }
                else
                {
                    OnTraceEnd();
                }
            }
            //update mousePos log
            m_previousDrawPos = drawTo;
        }

    }
}
