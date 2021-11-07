using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class AnimatorHandler : MonoBehaviour
    {
        [SerializeField] Animator m_animator = default;
        
        public void OnWindUp() 
        {
            m_animator.SetBool("DashCharging", true);
            m_animator.SetTrigger("DashTrigger");
        }

        public void OnDashCancel() 
        { 
        
        }

        public void OnDashRelease() 
        {
            m_animator.SetBool("DashCharging", false);
        }

        public void OnTakeDamage() 
        {
            m_animator.SetTrigger("TakeDamage");      
        }

    }
}
