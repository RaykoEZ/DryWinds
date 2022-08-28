using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    public interface ICrisis 
    {
        void Init();
        void CrisisUpdate(float dt);
        void End();
    }
    public class Sandstorm : MonoBehaviour, ICrisis
    {
        [SerializeField] RadialRangeRenderer m_range = default;
        [SerializeField] SandstormProperty m_property = default;
        protected float m_timeElapsed = 0f;
        HashSet<BaseCharacter> m_inside = new HashSet<BaseCharacter>();
        Vector2 m_direction = Vector2.up;
        public SandstormProperty Property { get { return m_property; } set { m_property = value; } }

        public virtual void Init() 
        {
        }

        private void Start()
        {
            StartCoroutine(Grow());
        }
        public virtual void CrisisUpdate(float dt)
        {
            
        }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out BaseCharacter result))
            {
                m_inside.Add(result);
            }

        }

        protected virtual void OnTriggerExit2D(Collider2D col)
        {
            if (col.TryGetComponent(out BaseCharacter result))
            {
                m_inside.Remove(result);
            }
        }

        protected virtual void Move() 
        {
            Vector3 diff = Time.deltaTime * Property.Intensity * m_direction.normalized;
            transform.position += diff;
        }
        public virtual void End() 
        {
            Destroy(gameObject);
        }
        protected virtual IEnumerator Grow() 
        {
            yield return new WaitUntil(() => { return m_range != null; });
            m_range.Radius = Property.StartRadius;
            float r;
            while (m_timeElapsed <= Property.Life) 
            {
                r = Mathf.Clamp(m_range.Radius + (Time.deltaTime * Property.GrowthRate),
                        0.1f, 20f);
                m_range.Radius = r;
                m_timeElapsed += Time.deltaTime;
                Move();
                yield return null;
            }
            End();
        }

    }
}
