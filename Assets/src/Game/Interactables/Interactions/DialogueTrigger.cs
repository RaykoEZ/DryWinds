using System.Collections.Generic;
using UnityEngine;
using Curry.UI;
using Curry.Events;

namespace Curry.Game
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] CurryGameEventSource m_onDialogue = default;
        public void TriggerDialogue(EncounterResult dialogue, bool displayName = true)
        {
            Dictionary<string, object> payload = new Dictionary<string, object>
            {
                {"dialogue", dialogue },
                {"displayName", displayName }
            };
            EventInfo info = new EventInfo(payload);
            m_onDialogue.Broadcast(info);
        }
    }
}
