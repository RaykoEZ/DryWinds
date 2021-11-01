using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Util;

namespace Curry.Skill
{
    public delegate void OnActivateDrawSkill(RegionInput input);
    // Trace is like the brush tip for paint tools but with decay behaviours, also detects patterns for skill activation
    [RequireComponent(typeof(EdgeCollider2D))]
    public class BaseTracer : Interactable
    {
        [SerializeField] protected LineRenderer m_lineRenderer = default;
        [SerializeField] protected EdgeCollider2D m_edgeCollider = default;

        public event OnActivateDrawSkill OnActivate;

        protected bool m_loopTriggered = false;
        protected Queue<Vector2> m_drawnVert = new Queue<Vector2>();
        protected Queue<Vector3> m_drawnPositions = new Queue<Vector3>();

        protected override void OnCollisionEnter2D(Collision2D col)
        {
        }

        public override void Prepare()
        {
            ResetAll();
        }

        public virtual void OnTrace(Vector2 targetPosition, float length)
        {
            if (!m_drawnVert.Contains(targetPosition) && !m_loopTriggered)
            {
                m_drawnVert.Enqueue(targetPosition);
                m_drawnPositions.Enqueue(targetPosition);
                // We drew a shape/enclosure, we set the trace to the drawn shape
                if (DetectPattern(targetPosition)) 
                {
                    List<Vector2> trimmedVerts = GetTrimmmedPattern(m_drawnVert.ToArray(), targetPosition);
                    if (GameUtil.AreaOfEnclosure(trimmedVerts.ToArray()) > 1f) 
                    {
                        TracePattern(trimmedVerts);
                        return;
                    }
                }

                UpdateTrace(targetPosition);
            }
        }
        protected virtual void UpdateTrace(Vector2 targetPosition) 
        {
            m_lineRenderer.positionCount = m_drawnPositions.Count;
            m_lineRenderer.SetPosition(m_lineRenderer.positionCount - 1, targetPosition);
            m_edgeCollider.points = m_drawnVert.ToArray();
        }

        // Set the line render and collider to the shape we drew
        protected virtual void TracePattern(List<Vector2> shapeVerts) 
        {
            m_loopTriggered = true;
            m_drawnVert = new Queue<Vector2>(shapeVerts);
            Vector3[] newPos = VectorExtension.ToVector3Array(shapeVerts.ToArray());
            m_drawnPositions = new Queue<Vector3>(newPos);
            m_lineRenderer.positionCount = newPos.Length;
            m_lineRenderer.SetPositions(newPos);
            m_edgeCollider.points = m_drawnVert.ToArray();

            ActivateEffect(new List<Vector2>(m_drawnVert));
        }

        /// <summary>
        /// Determin whether the drawing activates the skill effect
        /// </summary>
        /// <returns> 
        /// true: the draw pattern will activate the skill effect
        /// false: no pattern detected on drawing yet
        /// </returns>
        protected virtual bool DetectPattern(Vector2 targetVert) 
        {
            bool ret = false;
            Vector2[] verts = m_edgeCollider.points;
            // need more than 1 points defined to have a general pattern.
            if(verts.Length > 0) 
            {
                Vector2 dir = (targetVert - verts[m_edgeCollider.points.Length - 1]).normalized;
                // move starting point from origin towards targetVert by 2x radius to avoid collision on origin.
                Vector2 start = verts[m_edgeCollider.points.Length - 1] + (2f * m_edgeCollider.edgeRadius * dir);
                float dist =
                    Vector2.Distance(targetVert, verts[m_edgeCollider.points.Length - 1]) >
                    Vector2.Distance(start, verts[m_edgeCollider.points.Length - 1]) ? Vector2.Distance(targetVert, start) : 0f;

                // 1 << 8 for drawing layer, CONST IN THE FUTURE
                RaycastHit2D hit = Physics2D.CircleCast(start, m_edgeCollider.edgeRadius, dir, dist, 1 << 8);
                ret = hit.collider == m_edgeCollider;
            }
            return ret;
        }

        /// <summary>
        /// Gets the list of verts we drew that made the enclosure, gets rid of hanging edges.
        /// </summary>
        /// <param name="verts"> the point positions of the drawn line</param>
        /// <param name="closureVert"> the latest point to make contact with the line to form a shape</param>
        /// <param name="searchRadius"> the radius for searching neighbourhood points</param>
        /// <returns></returns>
        protected List<Vector2> GetTrimmmedPattern(Vector2[] verts, Vector2 closureVert, float searchRadius = 0.1f) 
        {
            List<Vector2> toKeep = new List<Vector2>(verts);
            toKeep[0] = closureVert;
            if (verts.Length > 2) 
            {
                List<Vector2> toRemove = new List<Vector2>();

                for (int i = 0; i < verts.Length - 1; ++i) 
                {
                    // From first vert to the closest vert to closure point, remove those hanging verts and keep the rest
                    // Stop on the first instance of finding the closest point to closure point
                    toRemove.Add(verts[i]);
                    searchRadius *= 1.2f;
                    Vector2 midPoint = Vector2.Lerp(verts[i], verts[i + 1], 0.5f);
                    bool isMidPointClose = Vector2.Distance(midPoint, closureVert) < searchRadius;
                    bool isNearClosure = Vector2.Distance(verts[i], closureVert) < searchRadius;
                    if (isNearClosure || isMidPointClose) 
                    {
                        break;
                    }
                }   

                foreach(Vector2 remove in toRemove) 
                {
                    toKeep.Remove(remove);
                }
            }
            return toKeep;
        } 

        protected virtual void ActivateEffect(List<Vector2> input) 
        {
            RegionInput payload = new RegionInput(input);
            OnActivate?.Invoke(payload);
        }

        public virtual void OnClear()
        {
            StartCoroutine(OnExit());
        }

        protected virtual IEnumerator OnExit() 
        {
            yield return new WaitForSeconds(1.0f);
            ResetAll();
            ReturnToPool();
        }

        protected virtual void ResetAll() 
        {
            OnActivate = null;
            m_loopTriggered = false;
            m_lineRenderer.positionCount = 0;
            m_drawnVert.Clear();
            m_drawnPositions.Clear();
            m_edgeCollider.points = m_drawnVert.ToArray();
        }
    }
}
