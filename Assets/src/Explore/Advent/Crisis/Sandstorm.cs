using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    public abstract class CrisisBehaviour : MonoBehaviour, IPoolable
    {
        public IObjectPool Origin { get; set; }

        public abstract void Init(CrisisProperty prop);
        public abstract void CrisisUpdate(float dt);
        public abstract void End();

        public void Prepare()
        {
            throw new System.NotImplementedException();
        }

        public void ReturnToPool()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Sandstorm : CrisisBehaviour
    {
        [SerializeField] RadialRangeRenderer m_range = default;
        [SerializeField] SandstormProperty m_property = default;
        protected float m_timeElapsed = 0f;
        HashSet<BaseCharacter> m_inside = new HashSet<BaseCharacter>();
        Vector2 m_direction = Vector2.up;
        public SandstormProperty Property { get { return m_property; } set { m_property = value; } }

        public override void Init(CrisisProperty prop) 
        {
            if( prop is SandstormProperty sandstorm ) 
            {
                Property = sandstorm;
            }
        }

        private void Start()
        {
            StartCoroutine(Grow());
        }
        public override void CrisisUpdate(float dt)
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
        public override void End() 
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
