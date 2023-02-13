using Curry.Util;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Curry.Explore
{
    // A UI popup controlled by GamePhaseManager to display notice for change in game phase
    public class GamePhaseChangePopup : MonoBehaviour
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_popupLabel = default;
        [SerializeField] CoroutineManager m_popup = default;
        public void ShowPopup(string phaseName, Action onFinish) 
        {
            m_popup.ScheduleCoroutine(PhaseChange_Internal(phaseName, onFinish), interruptNow: true);
            m_popup.StartScheduledCoroutines();
        }

        IEnumerator PhaseChange_Internal(string phaseName, Action onFinish) 
        {
            m_popupLabel.text = phaseName;
            m_anim.SetTrigger("Show");
            yield return new WaitForSeconds(m_anim.GetCurrentAnimatorStateInfo(0).length);
            yield return new WaitForSeconds(0.1f);
            m_anim.ResetTrigger("Show");
            onFinish?.Invoke();
        }
    }

}