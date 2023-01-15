using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Curry.Explore
{
    // A UI popup controlled by GamePhaseManager to display notice for change in game phase
    public class GamePhaseChangePopup : SceneInterruptBehaviour
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_popupLabel = default;
        bool m_inProgress = false;
        public bool AnimationInProgress => m_inProgress && 
            m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
        
        public void ShowPopup(string phaseName, Action onFinish) 
        {
            m_inProgress = true;
            m_popupLabel.text = phaseName;
            StartCoroutine(PhaseChange_Internal(onFinish));
        }

        IEnumerator PhaseChange_Internal(Action onFinish) 
        {
            StartInterrupt();
            m_anim.SetTrigger("Show");
            yield return new WaitWhile(() => AnimationInProgress);
            yield return new WaitForSeconds(0.05f);
            m_anim.ResetTrigger("Show");
            m_inProgress = false;
            onFinish?.Invoke();
            EndInterrupt();
        }
    }

}