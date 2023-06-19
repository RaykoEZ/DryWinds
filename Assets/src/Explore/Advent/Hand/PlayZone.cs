using Curry.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.Explore
{
    public class PlayZone : CardDropZone 
    {
        [SerializeField] Image m_playPanel = default;
        public void SetPlayZonrActive(bool playable = true) 
        {
            m_playPanel.enabled = playable;
        }
    }
}