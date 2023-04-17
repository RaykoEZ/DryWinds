using System;
using UnityEngine;

namespace Curry.UI
{
    [Serializable]
    public class PanelUIHandler 
    {
        [SerializeField] protected Animator m_anim = default;
        public virtual void Show() 
        {
            m_anim.SetBool("show", true);
        }
        public virtual void Hide() 
        {
            m_anim.SetBool("show", false);
        }
    }
}