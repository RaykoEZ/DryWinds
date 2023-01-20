using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Curry.Util;
using Curry.Events;
using static Curry.Explore.DraggableCard;
using System.Collections;

namespace Curry.Explore
{
    public delegate void OnEffectActivate(int timeSpent, List<IEnumerator> onActivate = null);
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
            m_hand.Init(m_player, m_player.transform);
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
            m_playZone.OnDropped += OnCardPlay;
            m_hand.OnCardTargetResolve += OnCardPlay;
        }
        public void DisablePlay() 
        {
            m_playZone.OnDropped -= OnCardPlay;
            m_hand.OnCardTargetResolve -= OnCardPlay;
        }
        void OutOfTime(int timeSpent) 
        {
            Debug.Log("Out of Time");
        }
        // When card is trying to actvated after it is dropped...
        void OnCardPlay(AdventCard card, Action onPlay, Action onCancel) 
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
                List<IEnumerator> actions = new List<IEnumerator>
                {
                    m_hand.PlayCard(card, m_player.CurrentStats)
                };
                OnActivate?.Invoke(card.TimeCost, actions);
            }
            else 
            {
                m_hand.HidePlayZone();
                onCancel?.Invoke();
            }
        }
    }
}