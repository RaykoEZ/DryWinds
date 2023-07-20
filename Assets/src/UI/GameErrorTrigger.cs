using Curry.Events;
using UnityEngine;

namespace Curry.UI
{
    public static class ErrorMessages 
    {
        public static string s_notEnoughTime = "Insufficient Time";
        public static string s_notEnoughAp = "Insufficient AP";
        public static string s_handLimitExceed = "Insufficient Hand Capacity";
        public static string s_chosenTooFew = "Chosen too few";
        public static string s_chosenTooMany = "Chosen too many";
    }
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