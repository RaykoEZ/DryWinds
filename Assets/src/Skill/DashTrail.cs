using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class DashTrail : MonoBehaviour
    {
        [SerializeField] protected TrailRenderer m_trail = default;

        public virtual void OnDashRelease() 
        {
            m_trail.emitting = true;
        }
        
        public virtual void OnDashStop() 
        {
            m_trail.emitting = false;
        }
    }
}
