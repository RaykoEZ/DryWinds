using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Curry.Game;
using Curry.Util;

namespace Curry.Skill
{
    // Trace is the brush tip for paint tools
    public class BaseTracer : Interactable
    {
        [SerializeField] protected LineRenderer m_lineRenderer = default;
        [SerializeField] protected Brush m_brush = default;
        protected Queue<Vector2> m_drawnVert = new Queue<Vector2>();
        protected Queue<Vector3> m_drawnPositions = new Queue<Vector3>();
        protected float m_drawnLength = 0f;
        protected Vector2 m_prev;
        public Vector2[] Verts { get { return m_drawnVert.ToArray(); } }
        public float Length { get { return m_drawnLength; } }
        public override void Prepare()
        {
            ResetAll();
            m_brush.Init();
        }

        public virtual bool OnTrace(Vector2 newPosition)
        {
            m_brush.Show(newPosition);
            bool blocked = m_brush.IsBlocked;
            if (!blocked) 
            {
                AddVertex(newPosition);
                m_prev = newPosition;
            }
            return !blocked;
        }

        protected void AddVertex(Vector2 targetPosition) 
        {        
            if (!m_drawnVert.Contains(targetPosition))
            {
                m_drawnVert.Enqueue(targetPosition);
                m_drawnPositions.Enqueue(targetPosition);
                UpdateTrace(targetPosition);
                if (m_drawnVert.Count > 0) 
                {
                    m_drawnLength += Vector2.Distance(m_drawnVert.Peek(), targetPosition);
                }
            }
        }

        protected virtual void UpdateTrace(Vector2 targetPosition) 
        {
            m_lineRenderer.positionCount = m_drawnPositions.Count;
            m_lineRenderer.SetPosition(m_lineRenderer.positionCount - 1, targetPosition);
        }

        public virtual void OnClear()
        {
            ResetAll();
            ReturnToPool();
        }

        protected virtual void ResetAll() 
        {
            transform.position = Vector3.zero;
            m_brush.Hide();
            m_drawnLength = 0f;
            m_lineRenderer.positionCount = 0;
            m_drawnVert.Clear();
            m_drawnPositions.Clear();
        }
    }
}
