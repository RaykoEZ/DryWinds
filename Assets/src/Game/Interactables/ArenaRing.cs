using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    [RequireComponent(typeof(Collider2D))]
    public class ArenaRing : DynamicInteractable
    {
        [SerializeField] float m_initScale = default;
        [SerializeField] float m_duration = default;
        protected Coroutine m_scaling;
        protected Coroutine m_moving;

        protected void Start()
        {
            Scale(m_initScale);
        }

        public void Scale(float scale) 
        {
            if (m_scaling != null) 
            {
                StopCoroutine(m_scaling);
                m_scaling = null;
            }
            m_scaling = StartCoroutine(LerpToScale(scale));
        }

        public void Move(Vector3 target)
        {
            if (m_moving != null)
            {
                StopCoroutine(m_moving);
                m_moving = null;
            }
            m_moving = StartCoroutine(LerpToLocation(target));
        }

        protected IEnumerator LerpToScale(float targetScale)
        {
            float t = 0f;
            float current;
            while (t < m_duration)
            {
                current = Mathf.Lerp(transform.localScale.x, targetScale, t / m_duration);
                SetRingScale(current);
                t += Time.deltaTime;
                yield return null;
            }
            m_scaling = null;
            yield return null;
            UpdatePathfinder();
        }

        protected IEnumerator LerpToLocation(Vector3 target)
        {
            float t = 0f;
            while (t < m_duration)
            {
                transform.position = Vector3.Lerp(transform.position, target, t / m_duration);
                t += Time.deltaTime;
                yield return null;
            }
            m_moving = null;
            yield return null;
            UpdatePathfinder();
        }

        protected void SetRingScale(float scale) 
        {
            transform.localScale = new Vector3(scale, scale, scale);
            EdgeCollider2D col = (EdgeCollider2D)m_hurtBox;
            col.edgeRadius = 0.5f * scale;
        }
    }
}
