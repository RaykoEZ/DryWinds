using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Curry.Util;
using Curry.Events;

namespace Curry.Explore
{
    public delegate void OnEffectActivate(int timeSpent, List<Action> onActivate = null);
    public delegate void OnCardPlayed(AdventCard played);
    public class PlayManager : MonoBehaviour
    {
        [SerializeField] TimeManager m_time = default;
        [SerializeField] CardDropZone m_playZone = default;
        [SerializeField] HandZone m_hand = default;
        [SerializeField] Adventurer m_player = default;
        [SerializeField] CurryGameEventListener m_onCardBeginDrag = default;
        [SerializeField] CurryGameEventListener m_onCardDropped = default;
        [SerializeField] Image m_playPanel = default;
        public event OnEffectActivate OnActivate;
        protected void Start()
        {
            m_onCardBeginDrag?.Init();
            m_onCardDropped?.Init();
            m_hand.OnEncounterTrigger += OnEncounterDraw;
        }
        void OnEnable()
        {
            EnablePlay();
            m_time.OnOutOfTimeTrigger += OutOfTime;
        }

        void OnDisable()
        {
            DisablePlay();
            m_time.OnOutOfTimeTrigger -= OutOfTime;
        }
        
        public void ShowPlayZone()
        {
            m_playPanel.enabled = true;
        }
        public void HidePlayZone()
        {
            m_playPanel.enabled = false;
        }

        public void EnablePlay()
        {
            m_playZone.OnDropped += OnCardPlayed;
            m_onCardBeginDrag?.Init();
            m_onCardDropped?.Init();
        }
        public void DisablePlay() 
        {
            m_playZone.OnDropped -= OnCardPlayed;
            m_onCardBeginDrag?.Shutdown();
            m_onCardDropped?.Shutdown();
        }
        void OutOfTime(int timeSpent) 
        {
            Debug.Log("Out of Time");
        }
        // When card is trying to actvated after it is dropped...
        void OnCardPlayed(AdventCard card, Action onPlay, Action onCancel) 
        {
            //Try Spending Time/Resource, if not able, cancel
            m_time.TrySpendTime(card.TimeCost, out bool enoughTime);
            if (enoughTime) 
            {
                onPlay?.Invoke();
                List<Action> actions = new List<Action>();
                actions.Add(
                    () => {
                        m_hand?.PlayCard(card, m_player.Stats);
                    }
                    );
                OnActivate?.Invoke(card.TimeCost, actions);
            }
            else 
            {
                onCancel?.Invoke();
            }
            HidePlayZone();
        }
        void OnEncounterDraw(IReadOnlyList<Encounter> draw) 
        {
            foreach (Encounter encounter in draw) 
            {
                m_time.TrySpendTime(encounter.TimeCost, out bool _);
                encounter?.CardEffect?.Invoke(m_player.Stats);
            }
        }
    }
}