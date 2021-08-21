using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI
{
    public class OverlayAnimatorHandler : MonoBehaviour
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] float m_idleWait = default;
        Coroutine m_scheduledHide = default;

        // Call this as soon as UI finishes value update
        public void SceduleHide() 
        {
            if(m_scheduledHide != null) 
            {
                StopCoroutine(m_scheduledHide);
            }

            m_scheduledHide = StartCoroutine(HideHUD(m_idleWait));
        }

        public void OnShow() 
        {
            if (m_scheduledHide != null) 
            {
                StopCoroutine(m_scheduledHide);
            }

            m_anim.SetBool("HUDShow", true);
        }

        IEnumerator HideHUD(float waitForSec)
        {
            yield return new WaitForSeconds(waitForSec);
            m_anim.SetBool("HUDShow", false);
            m_scheduledHide = null;
        }
    }
}
