using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;
using TMPro;
using Curry.Game;

namespace Curry.UI
{
    [Serializable]
    public struct ChoiceConditions 
    {
        [SerializeField] bool m_canCancel;
        [Range(0, int.MaxValue)]
        [SerializeField] int m_maxChoice;
        [Range(0, int.MaxValue)]
        [SerializeField] int m_minChoice;
        public bool CanCancel { get { return m_canCancel; } }
        public int MaxChoiceCount { get { return Mathf.Max(m_maxChoice, 0); } }
        public int MinChoiceCount { get { return Mathf.Clamp(m_minChoice, 0, MaxChoiceCount); } }
        public ChoiceConditions(bool canCnacel, int max, int min)
        {
            m_canCancel = canCnacel;
            m_maxChoice = max;
            m_minChoice = min;
        }
    }
    // The fields to fill the choice panel
    // (e.g: the pool of items to choose from, title, max/min to choose...etc)
    public struct ChoiceContext
    {
        ChoiceConditions m_conditions;
        List<IChoice> m_availableChoices;
        public ChoiceConditions Conditons { get { return m_conditions; } }
        public List<IChoice> ChooseFrom { 
            get { return m_availableChoices; } 
            set { m_availableChoices = value; } }
        public ChoiceContext(ChoiceConditions conditions, List<IChoice> availableChoices) 
        {
            m_conditions = conditions;
            m_availableChoices = availableChoices;
        }      
    }
    public struct ChoiceResult
    {
        public ChoiceStatus Status { get; set; }
        public IReadOnlyList<IChoice> Choices { get; set; }

        public ChoiceResult(ChoiceStatus status, List<IChoice> choices) 
        {
            Status = status;
            Choices = choices;
        }
        public enum ChoiceStatus
        {
            Confirmed,
            Cancelled
        }
    }

    public delegate void OnChoiceFinish(ChoiceResult result);
    public class ChoicePanelHandler : MonoBehaviour
    {
        [SerializeField] Animator m_panelAnim = default;
        [SerializeField] TextMeshProUGUI m_title = default;
        [SerializeField] Transform m_content = default;
        [SerializeField] Button m_confirm = default;
        [SerializeField] Button m_cancel = default;
        event OnChoiceFinish OnChoiceComplete;
        protected bool m_inProgress = false;
        protected ChoiceContext m_currentContext;
        protected List<IChoice> m_chosen = new List<IChoice>();
        public void BeginChoicePanel(ChoiceContext context) 
        {
            if (!m_inProgress) 
            {
                m_inProgress = true;
                m_currentContext = context;
                PrepareChoices();
            }
        }
        protected virtual void PrepareChoices() 
        {
            foreach (IChoice choice in m_currentContext.ChooseFrom) 
            {
                choice.DisplayChoice(m_content);
            }
        }
        public void ConfirmChoice() { }
        public void CancelChoice() { }
        void OnChoose(IChoice chosen) { }
        void OnUnchoose(IChoice chosen) { }
    }
}