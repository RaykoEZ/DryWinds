using System;
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

        void OutOfTime(float hourSpent) 
        {
            Debug.Log("Out of Time");
        }

        void OnCardPlayed(DraggableCard card) 
        {
            // Activate & Spend Time/Resource
            Action<Explorer> cardEffect = card?.Card.CardEffect;
            cardEffect?.Invoke(m_player);
        }
    }
}