using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;
using Curry.Events;

namespace Curry.Explore
{
    public class PlayManager : MonoBehaviour 
    {
        [SerializeField] TimeManager m_time = default;        
        [SerializeField] CardDropZone m_dropZone = default;
        [SerializeField] Explorer m_player = default;

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

        void OnCardPlayed(DraggableCard card) 
        {
            // Activate & Spend Time/Resource
            Action<Explorer> cardEffect = card?.Card.CardEffect;
            m_time.TrySpendTime(card.Card.TimeCost, out bool enoughTime);
            if (enoughTime) 
            {
                cardEffect?.Invoke(m_player);
            }
        }
    }
}