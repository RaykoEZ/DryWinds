using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    // Displays sonar visual effects and do detection checks
    public class SonarHandler : SceneInterruptBehaviour
    {
        [SerializeField] ParticleSystem m_scanRender = default;
        [SerializeField] ParticleSystem m_hitRender = default;
        [SerializeField] CircleCollider2D m_scanCollider = default;
        [SerializeField] ContactFilter2D m_scanFilter = default;
        [SerializeField] CurryGameEventListener m_onSonar = default;
        protected int m_currentDetectionLevel = 0;
        List<Transform> m_lastResults = new List<Transform>();
        #region Makeshift pooling for particle systems when scan hits an target
        int m_preLoad = 1;
        ParticleSystemPool m_hitPool = new ParticleSystemPool();
        #endregion
        public IReadOnlyList<Transform> LastScanHits => m_lastResults;
        float WaitTime => m_scanRender.main.duration;
        void Start()
        {
            m_onSonar?.Init();
            m_hitPool.Init(m_preLoad, m_hitRender, transform);
        }
        public void StartSonar(EventInfo info)
        {
            if (info is ScanInfo scan)
            {
                m_currentDetectionLevel = scan.DetectionLevel;
                Scan(scan.Diameter);
            }
        }
        public void Scan(float diameter)
        {
            // Set Range scale
            ParticleSystem.SizeOverLifetimeModule sizeModule = m_scanRender.sizeOverLifetime;
            sizeModule.sizeMultiplier = diameter;
            // Update radius, make sure we don't touch the edge of tiles out of our range
            m_scanCollider.radius = (0.48f * diameter);
            StartCoroutine(OnSonarScan());
        }
        protected virtual void DisplayHits() 
        {
            Debug.Log($" Scan Hits: {m_lastResults.Count} enemies");
            foreach (Transform t in m_lastResults)
            {
                // get particle sys
                ParticleSystem ps = m_hitPool.GetParticleSystem(t);
                ps?.Play();
            }
        }

        void ClearHits() 
        {
            m_lastResults.Clear();
            m_hitPool.ReturnAllToPool();
        }

        protected virtual void HandleDetection(IEnemy enemy, Transform transform) 
        {
            // Append hits if enemy is not stealthy (or has lower stealth level then scan level)
            if (enemy is IStealthy stealthy && stealthy.StealthLevel > m_currentDetectionLevel) 
            {
                return;
            }
            else 
            {
                m_lastResults.Add(transform);
            }
        }
        IEnumerator OnSonarScan()
        {
            StartInterrupt();
            ClearHits();
            List<Collider2D> results = new List<Collider2D>();            
            // Start scanning
            m_scanCollider.OverlapCollider(m_scanFilter, results);
            // Start trigger sonar particle system
            m_scanRender.Play();
            yield return new WaitForSeconds(WaitTime);
            // Start updating results
            foreach (Collider2D c in results)
            {
                if (c.attachedRigidbody.TryGetComponent(out IEnemy enemy))
                {
                    HandleDetection(enemy, c.transform);
                }
            }
            DisplayHits();
            EndInterrupt();
        }
    }
}
