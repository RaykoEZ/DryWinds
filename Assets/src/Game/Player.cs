using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;
using Curry.Events;

namespace Curry.Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] protected Camera m_cam = default;
        [SerializeField] protected PlayerContext m_playerContext = default;
        [SerializeField] BaseTracerBrush m_brush = default;

        protected int m_currentTraceIndex = 0;
        protected float m_regenTimer = 0f;
        protected Vector2 m_previousMousePos = default;
        protected PlayerContextFactory m_playerContextFactory = default;

        void Update()
        {
            if (m_playerContext.IsDirty) 
            {
                m_playerContextFactory.UpdateContext(m_playerContext);
            }
            OnPlayerTrace();
            OnSPRegen();
        }

        public void Init(PlayerContextFactory contextFactory)
        {
            m_playerContextFactory = contextFactory;
            m_playerContextFactory.UpdateContext(m_playerContext);
            m_playerContextFactory.Listen(OnPlayerContextUpdate);
            m_brush.EquipTrace(m_playerContext.CurrentTrace);
        }

        public void Shutdown() 
        {
            m_playerContextFactory.Unlisten(OnPlayerContextUpdate);
        }

        public void ChangeTrace(int index)
        {
            TraceAsset newTrace = m_playerContext.TraceInventory.GetTrace(index);
            if (newTrace != null)
            {
                m_playerContext.CurrentTrace = newTrace;
                m_currentTraceIndex = index;
                m_brush.EquipTrace(m_playerContext.CurrentTrace);
            }
        }

        public void NextTrace()
        {
            int newIndex = m_currentTraceIndex + 1;
            ChangeTrace(newIndex);
        }

        public void PreviousTrace()
        {
            int newIndex = m_currentTraceIndex - 1;
            ChangeTrace(newIndex);
        }

        protected void OnPlayerTrace() 
        {
            // current trace stroke ended?
            if (Input.GetMouseButtonDown(0))
            {
                m_brush.OnTraceEnd();
            }

            Vector2 mousePosition = m_cam.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButton(0) && mousePosition != m_previousMousePos)
            {
                float length = m_previousMousePos == null ? 0f : Vector2.Distance(mousePosition, m_previousMousePos);
                float SPCost = m_playerContext.CurrentTrace.TraceStats.SpCostScale * length;
                if (m_playerContext.CurrentStats.SP >= SPCost) 
                {
                    m_brush.Draw(mousePosition, length);
                    m_playerContext.CurrentStats.SP -= SPCost;
                }
                else 
                {
                    m_brush.OnTraceEnd();
                }
            }
            //update mousePos log
            m_previousMousePos = mousePosition;
        }

        protected void OnSPRegen() 
        {
            m_regenTimer += Time.deltaTime;
            if (m_regenTimer >= 1.0f && m_playerContext.CurrentStats.SP < m_playerContext.BaseStats.SP) 
            {
                m_regenTimer = 0f;
                float newSum = m_playerContext.CurrentStats.SP + m_playerContext.CurrentStats.SPRegenPerSec;
                m_playerContext.CurrentStats.SP = Mathf.Min(m_playerContext.BaseStats.SP, newSum);
            }
        }

        void OnPlayerContextUpdate(PlayerContext c) 
        {
            m_playerContext = c;
        }
    }
}
