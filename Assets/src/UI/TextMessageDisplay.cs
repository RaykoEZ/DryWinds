using Curry.Events;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Curry.UI
{
    public class GameMessageInfo : EventInfo 
    {
        string m_message = default;
        public string Message => m_message;
        public GameMessageInfo(string message)
        {
            m_message = message;
        }
    }
    // Controls error message animation
    public class TextMessageDisplay : GameMessageDisplay
    {
        [SerializeField] protected TextMeshProUGUI m_textField = default;
        public override void OnGameMessage(EventInfo info) 
        { 
            if (info is GameMessageInfo text && 
                !string.IsNullOrWhiteSpace(text.Message)) 
            {
                m_coroutine.ScheduleCoroutine(DisplayMessage_Internal(text.Message), m_coroutine.CoroutineInProgress);
            }
            if (!m_coroutine.CoroutineInProgress) 
            {
                m_coroutine.StartScheduledCoroutines();
            }
        }
        protected IEnumerator DisplayMessage_Internal(string message) 
        {
            m_textField.text = $"{message}";
            m_anim.ResetTrigger("show");
            m_anim.SetTrigger("show");
            yield return new WaitForSeconds(1.5f);
        }
    }
}