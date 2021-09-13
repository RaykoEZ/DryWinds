using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    // Trace is like the brush tip for paint tools but with decay behaviours
    public class BaseTrace : BaseSkill
    {
        [SerializeField] protected LineRenderer m_lineRenderer = default;
        // Units length to decay per decay interval.
        [SerializeField] protected float m_decayPerInterval = default;
        // Seconds to wait for next stroke decay interval
        [SerializeField] protected float m_decayWait = default;
        // Life of each drawn vertiex in seconds, starts to decay after this (sec)
        [SerializeField] protected float m_durability = default;

        protected bool m_isDecaying = false;
        protected bool m_isMakingCollider = true;
        protected Queue<Vector2> m_drawnVert = new Queue<Vector2>();
        protected Queue<Vector3> m_drawnPositions = new Queue<Vector3>();
        protected Queue<float> m_segmentLengths = new Queue<float>();       
        protected float m_decayTimer = 0f;

        protected EdgeCollider2D m_edgeCollider { get{ return (EdgeCollider2D)m_hitBox; } }
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

            if (hit == null || (hit.Relations & m_skillProperty.TargetOptions) == ObjectRelations.None)
            {
                return;
            }

            Vector2 dir = col.GetContact(0).normal.normalized;
            hit.OnKnockback(-dir , m_skillProperty.Knockback);
        }

        protected override IEnumerator SkillEffect(SkillParam target = null) 
        {
            yield break;
        }

        public override void Execute(SkillParam target)
        {
            float length = (float)target.Payload["length"];
            if (m_user?.CurrentStats.SP < length * m_skillProperty.SpCost)
            {
                return;
            }

            Vector2 targetPosition = target.TargetPos;
            if (!m_drawnVert.Contains(targetPosition))
            {
                EvaluateLength(length);
                m_drawnVert.Enqueue(targetPosition);
                m_drawnPositions.Enqueue(targetPosition);
                m_lineRenderer.positionCount = m_drawnPositions.Count;
                m_lineRenderer.SetPosition(m_lineRenderer.positionCount - 1, targetPosition);

                if (m_edgeCollider != null && m_isMakingCollider && m_drawnVert.Count > 1)
                {
                    m_edgeCollider.points = m_drawnVert.ToArray();
                }

                ConsumeResource(SkillProperties.SpCost * length);
            }  
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
            float decayAccel= 1.0f;
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

                yield return new WaitForSeconds(decayAccel * m_decayWait);
                decayAccel *= 0.5f;

            }
        }

        protected virtual void OnClear()
        {
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

            gameObject.SetActive(false);
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
