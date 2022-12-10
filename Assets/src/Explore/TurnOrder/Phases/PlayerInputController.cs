﻿using UnityEngine;
using Curry.Util;
namespace Curry.Explore
{
    // Enables/disables player input for turn orders
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] AdventButton m_adventureInput = default;
        [SerializeField] SelectionManager m_selectInput = default;
        [SerializeField] PlayManager m_cardPlay = default;
        [SerializeField] HandZone m_hand = default;
        private void Awake()
        {
            DisableInput();
        }
        public void EnableInput() 
        {
            m_adventureInput.Interactable = true;
            m_selectInput?.EnableSelection();
            m_cardPlay?.EnablePlay();
        }
        public void DisableInput()
        {
            m_adventureInput.Interactable = false;
            m_selectInput?.DisableSelection();
            m_cardPlay?.DisablePlay();
        }
        public void DiscardPlayerHand() 
        {
            m_hand?.DiscardHand();
        }
    }
}