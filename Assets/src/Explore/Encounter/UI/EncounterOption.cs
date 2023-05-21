using Curry.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Curry.Explore
{
    [Serializable]
    public class EncounterOption : MonoBehaviour
    {
        [SerializeField] Button m_button = default;
        [SerializeField] TextMeshProUGUI m_descriptionText = default;
        [SerializeField] TextMeshProUGUI m_detailText = default;
        protected EncounterOptionAttribute m_details;
        public delegate void OnEncounterChosen(EncounterOptionAttribute chosen);
        public event OnEncounterChosen OnChosen;
        protected GameStateContext m_context;

        public virtual void Init(EncounterOptionAttribute details, GameStateContext context) 
        {
            m_context = context;
            m_details = details;
            ShowDescription();
            m_button.interactable = true;
        }
        public virtual void OnPointerClick()
        {
            m_button.interactable = false;
            OnChosen?.Invoke(m_details);
        }
        protected virtual void ShowDescription() 
        {
            m_descriptionText.text = m_details.Description;
            m_detailText.text = m_details.OutcomeDetail.DetailText;
        }
    }
}