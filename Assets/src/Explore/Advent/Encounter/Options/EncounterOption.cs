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
        [SerializeField] string m_description = default;
        [SerializeField] string m_detail = default;
        [SerializeField] Animator m_anim = default;
        [SerializeField] Button m_button = default;
        [SerializeField] Encounter m_encounter = default;
        [SerializeField] TextMeshProUGUI m_descriptionText = default;
        public delegate void OnEncounterChosen(Dialogue dialogue);
        public event OnEncounterChosen OnChosen;
        public string Description => m_description;
        protected GameStateContext m_context;
        public virtual bool IsOptionAvailable { get; protected set; } = true;

        public virtual void Init(GameStateContext context) 
        {
            m_context = context;
            ShowDescription();
            m_button.interactable = true;
        }
        public virtual void OnPointerClick()
        {
            m_button.interactable = false;
            Dialogue result = Choose(m_context);
            OnChosen?.Invoke(result);
        }
        public virtual bool IsAvailable(GameStateContext conditions)
        {
            return true;
        }
        protected virtual void ShowDetail()
        {
            if (!string.IsNullOrWhiteSpace(m_detail)) 
            {
                m_descriptionText.text = m_detail;
            }
        }
        protected virtual void ShowDescription() 
        {
            m_descriptionText.text = m_description;
        }
        protected virtual void HideDetail()
        {
            ShowDescription();
        }
        protected virtual Dialogue Choose(GameStateContext context)
        {
            return m_encounter.OnChoose(context);
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