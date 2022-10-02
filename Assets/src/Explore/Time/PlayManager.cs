using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Curry.Util;
using Curry.Events;

namespace Curry.Explore
{
    public class PlayManager : MonoBehaviour 
    {
        [SerializeField] TimeManager m_time = default;        
        [SerializeField] CardDropZone m_dropZone = default;
        [SerializeField] Adventurer m_player = default;
        [SerializeField] CurryGameEventListener m_onCardBeginDrag = default;
        [SerializeField] CurryGameEventListener m_onCardDropped = default;
        [SerializeField] Image m_playPanel = default;
        protected void Awake()
        {
            m_onCardBeginDrag.Init();
            m_onCardDropped.Init();
        }
        void OnEnable()
        {
            m_time.OnOutOfTimeTrigger += OutOfTime;
            m_dropZone.OnDropped += OnCardPlayed;
        }

        void OnDisable()
        {
            m_time.OnOutOfTimeTrigger -= OutOfTime;
            m_dropZone.OnDropped -= OnCardPlayed;
        }

        void OutOfTime(int timeSpent) 
        {
            Debug.Log("Out of Time");
        }

        public void ShowPlayZone()
        {
            m_playPanel.enabled = true;
        }
        public void HidePlayZone()
        {
            m_playPanel.enabled = false;
        }

        void OnCardPlayed(DraggableCard card) 
        {
            // Activate & Spend Time/Resource
            Action<Adventurer> cardEffect = card?.Card.CardEffect;
            m_time.TrySpendTime(card.Card.TimeCost, out bool enoughTime);
            if (enoughTime) 
            {
                cardEffect?.Invoke(m_player);
            }
            HidePlayZone();
        }
    }
}