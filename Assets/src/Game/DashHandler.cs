using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Curry.Skill;

namespace Curry.Game
{
    public class DashHandler : MonoBehaviour
    {
        [SerializeField] Rigidbody2D m_rigidbody = default;
        [SerializeField] DashTrail m_trail = default;
        [SerializeField] float m_dashCDTime = default;
        [SerializeField] float m_maxWindupTime = default;
        [SerializeField] int m_maxDashCount = default;

        bool m_isDashOnCD = false;
        bool m_isDashWindingUp = false;
        int m_currentDashCount = 0;
        float m_dashWindupTimer = 0f;

        public bool IsDashAvailable { 
            get 
            { 
                return m_currentDashCount > 0; 
            } 
        }

        void Start()
        {
            m_currentDashCount = m_maxDashCount;
        }

        void Update()
        {
            // When rmb held, charge dash guage
            if (
                Mouse.current.rightButton.isPressed 
                && !m_isDashOnCD 
                && m_isDashWindingUp
                )
            {
                m_dashWindupTimer += Time.deltaTime;
            }
        }

        public virtual void DashWindup() 
        {
            m_isDashWindingUp = true;
        }

        public virtual void Dash(float speed)
        {
            StartCoroutine(OnDashCD());
            Debug.Log(m_dashWindupTimer);
            float chargeFactor = Mathf.Max(
            0.5f,
            (Mathf.Min(m_dashWindupTimer, m_maxWindupTime)) / m_maxWindupTime);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            m_dashWindupTimer = 0f;
            m_isDashWindingUp = false;
            StartCoroutine(DashMotion(m_rigidbody.position, mousePos, chargeFactor));
        }


        IEnumerator OnDashCD()
        {
            m_isDashOnCD = true;
            m_currentDashCount = Mathf.Max(0, --m_currentDashCount);
            yield return new WaitForSeconds(m_dashCDTime);
            m_currentDashCount = Mathf.Min(++m_currentDashCount, m_maxDashCount);
            m_isDashOnCD = false;
        }
    
    protected virtual IEnumerator DashMotion(Vector2 origin, Vector2 targetPos, float chargeCoeff) 
        {
            float t = 0f;
            float duration = 0.3f;
            Vector2 dir = targetPos - origin;
            m_trail.OnDashRelease();

            while (t < duration) 
            {
                m_rigidbody.AddForce(dir.normalized * chargeCoeff, ForceMode2D.Impulse);
                t += Time.deltaTime;
                yield return null;
            }

            t = 0f;
            while(t < duration) 
            {
                m_rigidbody.drag += 0.5f;
                t += Time.deltaTime;
                yield return null;
            }
            m_rigidbody.drag = 1f;
            m_trail.OnDashStop();
        }
    }
}
