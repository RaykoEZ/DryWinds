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
    // Intermediary between cards-in-hand and main play zone
    // Handles card activation validation
    public class PlayManager : MonoBehaviour
    {
        [SerializeField] TimeManager m_time = default;
        [SerializeField] CardDropZone m_playZone = default;
        [SerializeField] HandZone m_hand = default;
        [SerializeField] Adventurer m_player = default;
        public event OnEffectActivate OnActivate;
        protected void Start()
        {
            m_hand.Init(m_player);
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

        public void EnablePlay()
        {
            m_playZone.OnDropped += OnCardPlayed;
            m_hand.OnEncounterTrigger += OnEncounterDraw;
            m_hand.OnCardTargetResolve += OnCardPlayed;
        }
        public void DisablePlay() 
        {
            m_playZone.OnDropped -= OnCardPlayed;
            m_hand.OnEncounterTrigger -= OnEncounterDraw;
            m_hand.OnCardTargetResolve -= OnCardPlayed;
        }
        void OutOfTime(int timeSpent) 
        {
            Debug.Log("Out of Time");
        }
        // When card is trying to actvated after it is dropped...
        void OnCardPlayed(AdventCard card, Action onPlay = null, Action onCancel = null) 
        {
            if (!card.Activatable)
            {
                m_hand.HidePlayZone();
                onCancel?.Invoke();
                return;
            }
            //Try Spending Time/Resource, if not able, cancel
            m_time.TrySpendTime(card.TimeCost, out bool enoughTime);
            if (enoughTime) 
            {
                onPlay?.Invoke();
                // Make a container for the callstack and trigger it. 
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
                m_hand.HidePlayZone();
                onCancel?.Invoke();
            }
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