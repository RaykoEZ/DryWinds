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
        [SerializeField] AdventManager m_advent = default;
        [SerializeField] TimeManager m_time = default;
        [SerializeField] CardDropZone m_dropZone = default;
        [SerializeField] Adventurer m_player = default;
        [SerializeField] CurryGameEventListener m_onCardBeginDrag = default;
        [SerializeField] CurryGameEventListener m_onCardDropped = default;
        [SerializeField] CurryGameEventListener m_onCardDraw = default;
        [SerializeField] Image m_playPanel = default;
        protected void Awake()
        {
            m_onCardDraw?.Init();
            m_onCardBeginDrag?.Init();
            m_onCardDropped?.Init();
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
            Action<AdventurerStats> cardEffect = card?.Card.CardEffect;
            m_time.TrySpendTime(card.Card.TimeCost, out bool enoughTime);
            if (enoughTime) 
            {
                cardEffect?.Invoke(m_player.Stats);
            }
            HidePlayZone();
        }

        public void OnCardDrawn(EventInfo info) 
        {
            if (info == null) return;
            if(info is CardDrawInfo draw) 
            {
                OnEncounterDraw(draw.Encounters);
            }
        }

        void OnEncounterDraw(IReadOnlyList<Encounter> draw) 
        {
            int totalCost = 0;
            foreach(Encounter encounter in draw) 
            {
                totalCost += encounter.TimeCost;
                encounter?.OnDrawEffect(m_player.Stats);
            }
            m_time.TrySpendTime(totalCost, out bool _);
        }
    }
}