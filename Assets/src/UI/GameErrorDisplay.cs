using Curry.Events;
using Curry.Util;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Curry.UI
{
    public class GameErrorInfo : EventInfo 
    {
        string m_message = default;
        public string Message => m_message;

        public GameErrorInfo(string message)
        {
            m_message = message;
        }
    }
    // Controls error message animation
    public class GameErrorDisplay : MonoBehaviour
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_errorText = default;
        [SerializeField] CurryGameEventListener m_onGameError = default;
        [SerializeField] CoroutineManager m_coroutine = default;
        // Use this for initialization
        void Start()
        {
            m_onGameError.Init();
        }
        public void OnGameError(EventInfo info) 
        { 
            if (info is GameErrorInfo error && 
                !string.IsNullOrWhiteSpace(error.Message)) 
            {
                m_coroutine.ScheduleCoroutine(DisplayMessage_Internal(error.Message), m_coroutine.CoroutineInProgress);
            }
            if (!m_coroutine.CoroutineInProgress) 
            {
                m_coroutine.StartScheduledCoroutines();
            }
        }
        IEnumerator DisplayMessage_Internal(string message) 
        {
            m_errorText.text = message;
            m_anim.ResetTrigger("show");
            m_anim.SetTrigger("show");
            yield return new WaitForSeconds(1.5f);
        }
    }
}