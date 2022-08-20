using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    public class Sandstorm : MonoBehaviour 
    {
        [SerializeField] RadialRangeRenderer m_range = default;
        protected RadialCrisisProperty<BaseCharacter> m_crisis;
        protected float m_timeElapsed = 0f;
        float m_life = 10f;
        float m_intensity = 1f;
        float m_startRadius = 0.05f;
        float m_growthRate = 0.2f;

        public virtual void Init(RadialCrisisProperty<BaseCharacter> crisis) 
        {
            m_crisis = crisis;
            m_life = crisis.Life;
            m_intensity = crisis.Intensity;
            m_startRadius = crisis.StartRadius;
            m_growthRate = crisis.GrowthRate;
            m_range.Radius = m_startRadius;
            StartCoroutine(Grow());
        }

        private void Start()
        {
            StartCoroutine(Grow());
        }


        protected virtual IEnumerator Grow() 
        {
            yield return new WaitUntil(() => { return m_range != null; });
            m_range.Radius = m_startRadius;
            float r;
            while (m_timeElapsed <= m_life) 
            {
                r = Mathf.Clamp(m_range.Radius + (Time.deltaTime * m_growthRate),
                        0.1f, 20f);
                m_range.Radius = r;
                m_timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        protected virtual void FixedUpdate()
        {
            m_crisis?.OnCrisisUpdate(Time.deltaTime);
        }

        protected virtual void OnTriggerEnter2D(Collider2D col) 
        {
            if (col.TryGetComponent(out BaseCharacter result)) 
            {
                m_crisis?.OnEnterArea(result);
            }

        }

        protected virtual void OnTriggerExit2D(Collider2D col)
        {
            if (col.TryGetComponent(out BaseCharacter result))
            {
                m_crisis?.OnExitArea(result);
            }
        }
    }
}
