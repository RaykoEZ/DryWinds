using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    #region scan game event argument definitions
    public class ScanInfo : EventInfo
    {
        public ICharacter User { get; protected set; }
        public int DetectionLevel { get; protected set; }
        public float Diameter { get; protected set; }
        public Vector3 OriginWorldPosition { get; protected set; }
        public ScanInfo(ICharacter user, int detectionLevel, float diameter, Vector3 origin)
        {
            User = user;
            DetectionLevel = detectionLevel;
            Diameter = diameter;
            OriginWorldPosition = origin;
        }
    }
    #endregion
    // Displays sonar visual effects and do detection checks
    public class SonarHandler : SceneInterruptBehaviour
    {
        [SerializeField] ParticleSystem m_scanRender = default;
        [SerializeField] ParticleSystem m_hitRender = default;
        [SerializeField] CircleCollider2D m_scanCollider = default;
        [SerializeField] ContactFilter2D m_scanFilter = default;
        [SerializeField] CurryGameEventListener m_onSonar = default;
        protected int m_currentDetectionLevel = 0;
        protected ICharacter m_currentUser;
        List<Transform> m_lastResults = new List<Transform>();
        #region Makeshift pooling for particle systems when scan hits an target
        int m_preLoad = 1;
        ParticleSystemPool m_hitPool = new ParticleSystemPool();
        #endregion
        public IReadOnlyList<Transform> LastScanHits => m_lastResults;
        float WaitTime => m_scanRender.main.duration;
        void Start()
        {
            m_scanCollider.enabled = false;
            m_onSonar?.Init();
            m_hitPool.Init(m_preLoad, m_hitRender, transform);
        }
        public void StartSonar(EventInfo info)
        {
            if (info is ScanInfo scan)
            {
                m_currentUser = scan.User;
                transform.position = scan.OriginWorldPosition;
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
        protected virtual void HandleDetection(ICharacter hit, Transform transform) 
        {
            if(hit == m_currentUser) 
            {
                return;
            }
            // Append hits if detected character is not stealthy / stealth component is not stealthy enough
            // (stealth level is lower than scan level)
            if (hit is IStealthy stealthy && 
                stealthy.StealthLevel > m_currentDetectionLevel) 
            {
                return;
            }
            else if (transform.TryGetComponent(out IStealthy stealthComponent) && 
                stealthComponent.StealthLevel > m_currentDetectionLevel) 
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
            m_scanCollider.enabled = true;
            // Start scanning
            m_scanCollider.OverlapCollider(m_scanFilter, results);
            // Start trigger sonar particle system
            m_scanRender.Play();
            yield return new WaitForSeconds(WaitTime);
            // Start updating results
            foreach (Collider2D c in results)
            {
                if (c.attachedRigidbody.TryGetComponent(out ICharacter hit))
                {
                    HandleDetection(hit, c.transform);
                }
            }
            DisplayHits();
            EndInterrupt();
            m_currentUser = null;
            m_currentDetectionLevel = 0;
            m_scanCollider.enabled = false;
        }
    }
}
