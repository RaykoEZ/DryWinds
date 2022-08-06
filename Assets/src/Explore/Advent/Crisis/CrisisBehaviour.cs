using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    [RequireComponent(typeof(Collider2D))]
    public class CrisisBehaviour : MonoBehaviour 
    {
        protected Crisis m_crisis;

        public virtual void Init(Crisis crisis) 
        {
            m_crisis = crisis;
        }

        protected virtual void FixedUpdate()
        {
            m_crisis?.OnCrisisUpdate(Time.deltaTime);
        }

        protected virtual void OnTriggerEnter2D(Collider2D col) 
        {
            if (col.TryGetComponent(out Interactable result)) 
            {
                m_crisis?.OnEnterArea(result);
            }

        }

        protected virtual void OnTriggerExit2D(Collider2D col)
        {
            if (col.TryGetComponent(out Interactable result))
            {
                m_crisis?.OnExitArea(result);
            }
        }
    }
}
