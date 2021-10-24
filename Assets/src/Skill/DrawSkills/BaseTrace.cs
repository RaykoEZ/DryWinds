using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public delegate void OnTraceFinish();
    public delegate void OnActivateDrawSkill(List<Vector2> regionVerts);
    // Trace is like the brush tip for paint tools but with decay behaviours, also detects patterns for skill activation
    [RequireComponent(typeof(EdgeCollider2D))]
    public class BaseTrace : Interactable
    {
        [SerializeField] protected LineRenderer m_lineRenderer = default;
        [SerializeField] protected EdgeCollider2D m_edgeCollider = default;
        // Units length to decay per decay interval.
        [SerializeField] protected float m_decayPerInterval = default;
        // Seconds to wait for next stroke decay interval
        [SerializeField] protected float m_decayWait = default;
        // Life of each drawn vertiex in seconds, starts to decay after this (sec)
        [SerializeField] protected float m_durability = default;

        public event OnTraceFinish OnTracingFinish;
        public event OnActivateDrawSkill OnActivate;

        protected bool m_isDecaying = false;
        protected bool m_isMakingCollider = true;
        protected Queue<Vector2> m_drawnVert = new Queue<Vector2>();
        protected Queue<Vector3> m_drawnPositions = new Queue<Vector3>();
        protected Queue<float> m_segmentLengths = new Queue<float>();       
        protected float m_decayTimer = 0f;

        // Update is called once per frame
        protected virtual void FixedUpdate()
        {
            m_decayTimer += Time.deltaTime;
            if(m_decayTimer > m_durability && !m_isDecaying) 
            {
                m_isDecaying = true;
                StartCoroutine(OnDecay());
            }          
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            BaseCharacter hit = col.gameObject.GetComponent<BaseCharacter>();
            if (hit == null || (hit.Relations & m_relations) == ObjectRelations.None)
            {
                Vector2 dir = col.GetContact(0).normal.normalized;
                hit.OnKnockback(-dir, CurrentCollisionStats.Knockback);
            }
        }

        public override void Prepare()
        {
            ResetAll();
        }

        public virtual void Execute(Vector2 targetPosition, float length)
        {
            if (!m_drawnVert.Contains(targetPosition))
            {
                EvaluateLength(length);
                m_drawnVert.Enqueue(targetPosition);
                m_drawnPositions.Enqueue(targetPosition);
                DetectePattern(targetPosition);

                m_lineRenderer.positionCount = m_drawnPositions.Count;
                m_lineRenderer.SetPosition(m_lineRenderer.positionCount - 1, targetPosition);

                if (m_edgeCollider != null && m_isMakingCollider && m_drawnVert.Count > 1)
                {
                    m_edgeCollider.points = m_drawnVert.ToArray();
                }
            }
        }

        /// <summary>
        /// Determin whether the drawing activates the skill effect
        /// </summary>
        /// <returns> 
        /// true: the draw pattern will activate the skill effect
        /// false: no pattern detected on drawing yet
        /// </returns>
        protected virtual bool DetectePattern(Vector2 targetVert) 
        {
            bool ret = false;
            Vector2[] verts = m_edgeCollider.points;
            // need more than 1 points defined to have a general pattern.
            if(verts.Length > 0) 
            {
                Vector2 dir = (targetVert - verts[m_edgeCollider.points.Length - 1]).normalized;
                // move starting point from origin towards targetVert by 2x radius to avoid collision on origin.
                Vector2 start = verts[m_edgeCollider.points.Length - 1] + (1.5f * m_edgeCollider.edgeRadius * dir);
                float dist =
                    Vector2.Distance(targetVert, verts[m_edgeCollider.points.Length - 1]) >
                    Vector2.Distance(start, verts[m_edgeCollider.points.Length - 1]) ? Vector2.Distance(targetVert, start) : 0f;

                // 1 << 8 for drawing layer, CONST IN THE FUTURE
                RaycastHit2D hit = Physics2D.CircleCast(start, 0.5f * m_edgeCollider.edgeRadius, dir, dist, 1 << 8);
                if (hit.collider == m_edgeCollider) 
                {
                    Debug.Log($"loop point at {hit.point.ToString("F4")}, linecast from {start.ToString("F4")} to {targetVert.ToString("F4")}");
                }
            }
            return ret;
        }

        protected virtual void ActivateEffect(List<Vector2> regionVerts) 
        {
            OnActivate?.Invoke(regionVerts);
        }

        protected void EvaluateLength(float length)
        {
            int numP = m_lineRenderer.positionCount;
            if (numP == 0) 
            {
                return;
            }
            else 
            {
                //store it for later
                m_segmentLengths.Enqueue(length);
            }
        }

        protected virtual IEnumerator OnDecay()
        {
            float decayAmount = m_decayPerInterval;
            // if no more to decay, finish loop
            while (m_isDecaying)
            {
                if(m_segmentLengths.Count == 0) 
                {
                    OnClear();
                    yield break;
                }
                while (decayAmount > Mathf.Epsilon && m_segmentLengths.Count > 0)
                {
                    float lastSegmentLength = m_segmentLengths.Peek();
                    // if we can decay the segment and/or have more to decay, remove the last segment and keep going  
                    if (lastSegmentLength <= decayAmount)
                    {
                        m_drawnVert?.Dequeue();
                        m_drawnPositions?.Dequeue();
                        m_segmentLengths.Dequeue();
                        decayAmount -= lastSegmentLength;
                    }
                    else if (lastSegmentLength > decayAmount)
                    {
                        TrimEndSegment(decayAmount);
                        decayAmount = 0f;
                    }
                }
                // Reload the decay amount for next segment decay interval
                // With acceleration
                decayAmount += m_decayPerInterval;
                // Update line renderer and collider after decay
                m_lineRenderer.positionCount = m_drawnPositions.Count;
                m_lineRenderer.SetPositions(m_drawnPositions.ToArray());
                m_edgeCollider.points = m_drawnVert.ToArray();
                yield return new WaitForSeconds(m_decayWait);
            }
        }

        protected virtual void OnClear()
        {
            OnTracingFinish?.Invoke();
            ResetAll();
            ReturnToPool();
        }

        protected virtual void ResetAll() 
        {
            OnTracingFinish = null;
            OnActivate = null;
            m_isDecaying = false;
            m_decayTimer = 0f;
            m_lineRenderer.positionCount = 0;
            m_drawnVert.Clear();
            m_drawnPositions.Clear();
            m_segmentLengths.Clear();

            if (m_isMakingCollider)
            {
                m_edgeCollider.points = m_drawnVert.ToArray();
            }
        }

        protected void TrimEndSegment(float trimLength) 
        {
            if(m_segmentLengths.Count < 1) 
            { 
                return; 
            }

            Vector2[] vertList = m_drawnVert.ToArray();
            Vector3[] posList = m_drawnPositions.ToArray();
            float[] lengthList = m_segmentLengths.ToArray();
            // lerp last vertex to new position, shrinking this segment by $$decayAmount.
            Vector2 vlast = vertList[0];
            Vector2 vSecondLast = vertList[1];
            float t = trimLength / lengthList[0];
            Vector2 newVLast = Vector2.Lerp(vlast, vSecondLast, t);

            posList[0] = newVLast;
            vertList[0] = newVLast;
            lengthList[0] = Vector2.Distance(newVLast, vSecondLast);
            // update data structure with new values
            m_drawnVert = new Queue<Vector2>(vertList);
            m_drawnPositions = new Queue<Vector3>(posList);
            m_segmentLengths = new Queue<float>(lengthList);
        }
    }
}
