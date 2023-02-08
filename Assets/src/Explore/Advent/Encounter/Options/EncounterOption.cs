﻿using Curry.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Curry.Explore
{
    [Serializable]
    public class EncounterOption : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] Button m_button = default;
        [SerializeField] TextMeshProUGUI m_descriptionText = default;
        protected OptionDetail m_details;
        public delegate void OnEncounterChosen(EncounterResult dialogue);
        public event OnEncounterChosen OnChosen;
        protected GameStateContext m_context;
        public virtual bool IsOptionAvailable { get; protected set; } = true;

        public virtual void Init(OptionDetail details, GameStateContext context) 
        {
            m_context = context;
            m_details = details;
            ShowDescription();
            m_button.interactable = true;
        }
        public virtual void OnPointerClick()
        {
            m_button.interactable = false;
            EncounterResult result = Choose();
            OnChosen?.Invoke(result);
        }
        public virtual bool IsAvailable(GameStateContext conditions)
        {
            return true;
        }
        protected virtual void ShowDetail()
        {
            if (!string.IsNullOrWhiteSpace(m_details.Summary)) 
            {
                m_descriptionText.text = m_details.Summary;
            }
        }
        protected virtual void ShowDescription() 
        {
            m_descriptionText.text = m_details.Description;
        }
        protected virtual void HideDetail()
        {
            ShowDescription();
        }
        protected virtual EncounterResult Choose()
        {
            return m_details.Result;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowDetail();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideDetail();
        }
    }
}