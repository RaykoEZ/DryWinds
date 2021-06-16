using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class LineStroke : MonoBehaviour
    {
        [SerializeField] LineRenderer LineRenderer = default;
        [SerializeField] EdgeCollider2D EdgeCollider = default;
        // Units length to decay per decay interval.
        [SerializeField] float m_decayPerInterval = default;
        // Seconds to wait for next stroke decay interval
        [SerializeField] float m_decayWait = default;
        // Life of each drawn vertiex in seconds, starts to decay after this (sec)
        [SerializeField] float m_life = default;

        protected bool m_isDecaying = false;
        protected bool m_isMakingCollider = true;
        protected Queue<Vector2> m_drawnVert = new Queue<Vector2>();
        protected Queue<Vector3> m_drawnPositions = new Queue<Vector3>();
        protected Queue<float> m_segmentLengths = new Queue<float>();

        protected float m_decayTimer = 0f;

        // Update is called once per frame
        void FixedUpdate()
        {
            m_decayTimer += Time.deltaTime;
            if(m_decayTimer > m_life && !m_isDecaying) 
            {
                m_isDecaying = true;
                StartCoroutine(OnDecay());
            }          
        }

        public virtual void OverrideSetting(StrokeSetting setting) 
        { 
            
        
        }


        public virtual void OnDraw(Vector2 mousePosition)
        {
            if (Input.GetMouseButton(0))
            {
                if (!m_drawnVert.Contains(mousePosition))
                {
                    LogSegmentLength(mousePosition);

                    m_drawnVert.Enqueue(mousePosition);
                    m_drawnPositions.Enqueue(mousePosition);
                    LineRenderer.positionCount = m_drawnPositions.Count;
                    LineRenderer.SetPosition(LineRenderer.positionCount - 1, mousePosition);

                    if (EdgeCollider != null && m_isMakingCollider && m_drawnVert.Count > 1)
                    {
                        EdgeCollider.points = m_drawnVert.ToArray();
                    }
                }
            }
        }

        protected void LogSegmentLength(Vector2 p)
        {
            int numP = LineRenderer.positionCount;
            if (numP == 0) 
            {
                return;
            }
            // Get distance between new point and previous end point, store it for later
            Vector2 previous = LineRenderer.GetPosition(numP - 1);
            float segmentLength = Vector2.Distance(p, previous);
            m_segmentLengths.Enqueue(segmentLength);
        }

        protected virtual IEnumerator OnDecay()
        {
            float decayAmount = m_decayPerInterval;
            float decayAccel= 1.0f;
            // if no more to decay, finish loop
            while (m_isDecaying)
            {
                if(m_segmentLengths.Count == 0) 
                {
                    OnClear();
                    gameObject.SetActive(false);
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
                LineRenderer.positionCount = m_drawnPositions.Count;
                LineRenderer.SetPositions(m_drawnPositions.ToArray());
                EdgeCollider.points = m_drawnVert.ToArray();

                yield return new WaitForSeconds(decayAccel * m_decayWait);
                decayAccel *= 0.75f;

            }


        }

        void TrimEndSegment(float trimLength) 
        {
            if(m_segmentLengths.Count < 1) 
            { 
                return; 
            }

            Vector2[] vertList = m_drawnVert.ToArray();
            Vector3[] posList = m_drawnPositions.ToArray();
            float[] lengthList = m_segmentLengths.ToArray();
            // lerp last vertex to new position, shrinking theis segment by $$decayAmount.
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


        protected virtual void OnClear()
        {
            m_isDecaying = false;
            m_decayTimer = 0f;
            LineRenderer.positionCount = 0;
            m_drawnVert.Clear();
            m_drawnPositions.Clear();
            m_segmentLengths.Clear();

            if (m_isMakingCollider)
            {
                EdgeCollider.Reset();
            }
        }
    }
}
