using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Util;

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
        protected bool m_loopTriggered = false;
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
            if (!m_drawnVert.Contains(targetPosition) && !m_loopTriggered)
            {
                EvaluateLength(length);
                m_drawnVert.Enqueue(targetPosition);
                m_drawnPositions.Enqueue(targetPosition);
                // We drew a shape/enclosure, we set the trace to the drawn shape
                if (DetectPattern(targetPosition)) 
                {
                    List<Vector2> trimmedVerts = GetTrimmmedPattern(m_drawnVert.ToArray(), targetPosition, out int segRemoved);
                    if (GameUtil.AreaOfEnclosure(trimmedVerts.ToArray()) > 1f) 
                    {
                        SetShape(trimmedVerts, segRemoved);
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
        protected virtual void SetShape(List<Vector2> shapeVerts, int segmentsRemoved) 
        {
            m_loopTriggered = true;
            for (int i = 0; i < segmentsRemoved; ++i)
            {
                m_segmentLengths.Dequeue();
            }

            m_drawnVert = new Queue<Vector2>(shapeVerts);
            Vector3[] newPos = VectorExtension.ToVector3Array(shapeVerts.ToArray());
            m_drawnPositions = new Queue<Vector3>(newPos);
            m_lineRenderer.positionCount = newPos.Length;
            Debug.Log(newPos.Length);
            m_lineRenderer.SetPositions(newPos);
            m_edgeCollider.points = m_drawnVert.ToArray();
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
        protected List<Vector2> GetTrimmmedPattern(Vector2[] verts, Vector2 closureVert, out int segmentsRemoved, float searchRadius = 0.05f) 
        {
            List<Vector2> toKeep = new List<Vector2>(verts);
            toKeep[0] = closureVert;
            segmentsRemoved = -1;
            if (verts.Length > 2) 
            {
                List<Vector2> toRemove = new List<Vector2>();

                for (int i = 0; i < verts.Length; ++i) 
                {
                    // From first vert to the closest vert to closure point, remove those hanging verts and keep the rest
                    // Stop on the first instance of finding the closest point to closure point
                    toRemove.Add(verts[i]);
                    segmentsRemoved++;
                    searchRadius *= 1.5f;
                    bool isNearClosure = Vector2.Distance(verts[i], closureVert) < searchRadius;
                    if (isNearClosure) 
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
            m_loopTriggered = false;
            m_isDecaying = false;
            m_decayTimer = 0f;
            m_lineRenderer.positionCount = 0;
            m_drawnVert.Clear();
            m_drawnPositions.Clear();
            m_segmentLengths.Clear();
            m_edgeCollider.points = m_drawnVert.ToArray();
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
