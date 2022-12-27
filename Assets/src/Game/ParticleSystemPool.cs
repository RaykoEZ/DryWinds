using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    //Basic Pooling for a Particle System, instantiate another pool for a different particle system to pool
    public class ParticleSystemPool 
    {
        ParticleSystem m_loadRef;
        Transform m_defaultParent;
        List<ParticleSystem> m_hitPool = new List<ParticleSystem>();
        List<ParticleSystem> m_activeHits = new List<ParticleSystem>();
        public void Init(int numPreload, ParticleSystem psToLoad, Transform defaultParent) 
        {
            m_loadRef = psToLoad;
            m_defaultParent = defaultParent;
            //prepare particles systems
            for (int i = 0; i < numPreload; i++)
            {
                InstantiateHitParticles(m_loadRef, m_defaultParent);
            }
        }
        public ParticleSystem GetParticleSystem(Transform setParent) 
        {
            ParticleSystem ps;
            // get free particle sys in pool, or instantiate new ones if no free available
            if (m_hitPool.Count > 0)
            {
                ps = m_hitPool[0];
                m_activeHits.Add(ps);
                m_hitPool.RemoveAt(0);
                PrepareHitParticle(ps, setParent);
            }
            else
            {
                ps = InstantiateHitParticles(m_loadRef, setParent);
            }
            return ps;
        }
        public void ReturnToPool(ParticleSystem returning) 
        {
            if (m_activeHits.Remove(returning)) 
            {
                PrepareHitParticle(returning, m_defaultParent);
                m_hitPool.Add(returning);      
            }
        }
        public void ReturnAllToPool() 
        {
            foreach (ParticleSystem item in m_activeHits)
            {
                PrepareHitParticle(item, m_defaultParent);
                m_hitPool.Add(item);
            }
            m_activeHits.Clear();
        }
        ParticleSystem InstantiateHitParticles(ParticleSystem ps, Transform t)
        {
            GameObject go = Object.Instantiate(ps.gameObject);
            ParticleSystem ret = go.GetComponent<ParticleSystem>();
            PrepareHitParticle(ret, t);
            // Add to pool
            m_hitPool.Add(ret);
            return ret;
        }
        void PrepareHitParticle(ParticleSystem ps, Transform parent)
        {
            ps.Stop();
            ps.Clear();
            ps.transform.SetParent(parent);
            ps.transform.localPosition = -5f * Vector3.forward;
        }
    }
}
