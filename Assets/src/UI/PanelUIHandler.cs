using System;
using UnityEngine;

namespace Curry.UI
{
    [Serializable]
    public class PanelUIHandler 
    {
        [SerializeField] Animator m_anim = default;
        public void Show() 
        {
            m_anim.SetBool("show", true);
        }
        public void Hide() 
        {
            m_anim.SetBool("show", false);
        }
    }
}