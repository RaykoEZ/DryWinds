using Curry.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.UI
{
    public class ColouredMessageInfo : GameMessageInfo
    {
        Color32 m_backdropColour = Color.clear;
        public Color32 BackdropColour => m_backdropColour;
        public ColouredMessageInfo(string message, Color32 bgColor) : base(message)
        {
            m_backdropColour = bgColor;
        }
    }
    public class AbilityMessageDisplay : TextMessageDisplay
    {
        [SerializeField] protected Image m_backDrop = default;
        public override void OnGameMessage(EventInfo info)
        {
            if (info is ColouredMessageInfo ability &&
                !string.IsNullOrWhiteSpace(ability.Message))
            {
                Color color = ability.BackdropColour;
                color.a = m_backDrop.color.a;
                m_backDrop.color = color;
                m_coroutine.ScheduleCoroutine(DisplayMessage_Internal(ability.Message), m_coroutine.CoroutineInProgress);
            }
            if (!m_coroutine.CoroutineInProgress)
            {
                m_coroutine.StartScheduledCoroutines();
            }
        }
    }
}