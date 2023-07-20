using Curry.Events;
using UnityEngine;

namespace Curry.UI
{
    // Trigger error message event
    public class GameErrorTrigger : MonoBehaviour 
    {
        [SerializeField] CurryGameEventTrigger m_triggerMessage = default;
        public void SendErrorMessage(string errorMessage) 
        {
            GameErrorInfo info = new GameErrorInfo(errorMessage);
            m_triggerMessage?.TriggerEvent(info);
        }
    }
}