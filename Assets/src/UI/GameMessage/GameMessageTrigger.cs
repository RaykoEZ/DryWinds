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
    public class GameMessageTrigger : MonoBehaviour 
    {
        [SerializeField] CurryGameEventTrigger m_triggerMessage = default;
        public void TriggerGameMessage(string errorMessage) 
        {
            GameMessageInfo info = new GameMessageInfo(errorMessage);
            m_triggerMessage?.TriggerEvent(info);
        }
        public void TriggerGameMessage(string message, Color32 backdropColour) 
        {
            ColouredMessageInfo info = new ColouredMessageInfo(message, backdropColour);
            m_triggerMessage?.TriggerEvent(info);
        }
    }
}