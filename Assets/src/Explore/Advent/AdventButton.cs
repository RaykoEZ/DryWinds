using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Curry.Events;

namespace Curry.Explore 
{
    public class AdventButton : MonoBehaviour
    {
        [SerializeField] GameObject m_player = default;
        [SerializeField] CurryGameEventTrigger m_selectAdventureTile = default;

        public void PromptAdventurePosition() 
        {
            TileSelectionInfo info = new TileSelectionInfo(
                TileSelectionMode.Adventure,
                m_player,
                Camera.main.WorldToScreenPoint(m_player.transform.position));
            m_selectAdventureTile?.TriggerEvent(info);
        }
    }
}

