using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Curry.UI.ChoiceResult;
using Curry.Explore;

namespace Curry.UI
{
    #region structs for making choices
    [Serializable]
    public struct ChoiceConditions
    {
        [SerializeField] bool m_canCancel;
        [Range(0, 10)]
        [SerializeField] int m_maxChoice;
        [Range(0, 10)]
        [SerializeField] int m_minChoice;
        [SerializeField] string m_description;
        public bool CanCancel { get { return m_canCancel; } }
        public int MaxChoiceCount { get { return Mathf.Max(m_maxChoice, 0); } }
        public int MinChoiceCount { get { return Mathf.Clamp(m_minChoice, 0, MaxChoiceCount); } }
        public string Description { get { return m_description; } }
        public ChoiceConditions(bool canCnacel, int max, int min, string description)
        {
            m_canCancel = canCnacel;
            m_maxChoice = max;
            m_minChoice = min;
            m_description = description;
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
    #endregion

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
        protected HashSet<IChoice> m_chosen = new HashSet<IChoice>();
        bool CanConfirm => m_chosen?.Count >= m_currentContext.Conditons.MinChoiceCount;
        bool CanChoose => m_chosen?.Count < m_currentContext.Conditons.MaxChoiceCount;
        public void BeginChoicePanel(ChoiceContext context, OnChoiceFinish onFinish) 
        {
            if (!m_inProgress) 
            {
                // Clear all leftover choices
                foreach(Transform t in m_content) 
                {
                    Destroy(t.gameObject);
                }

                m_inProgress = true;
                OnChoiceComplete += onFinish;
                m_currentContext = context;
                m_title.text = context.Conditons.Description;
                m_confirm.interactable = false;
                m_cancel.interactable = context.Conditons.CanCancel;
                PrepareChoices();
                m_panelAnim.SetBool("show", true);
            }
        }
        protected virtual void PrepareChoices() 
        {
            foreach (IChoice choice in m_currentContext.ChooseFrom) 
            {
                choice.OnChosen += OnChoose;
                choice.OnUnchoose += OnUnchoose;
                choice.DisplayChoice(m_content);
            }
        }
        protected virtual void Clear() 
        {
            m_confirm.interactable = false;
            m_cancel.interactable = false;
            m_panelAnim.SetBool("show", false);
            foreach (IChoice choice in m_currentContext.ChooseFrom)
            {
                choice.OnChosen -= OnChoose;
                choice.OnUnchoose -= OnUnchoose;
            }
            m_currentContext = new ChoiceContext();
            OnChoiceComplete = null;
            m_chosen.Clear();
        }
        public void ConfirmChoice() 
        {
            ChoiceResult result = new ChoiceResult(ChoiceStatus.Confirmed, new List<IChoice>(m_chosen));
            OnChoiceComplete?.Invoke(result);
            Clear();
        }
        public void CancelChoice() 
        {
            ChoiceResult result = new ChoiceResult(ChoiceStatus.Cancelled, new List<IChoice>());
            OnChoiceComplete?.Invoke(result);
            Clear();
        }

        void OnChoose(IChoice chosen) 
        {
            if (CanChoose)
            {
                m_chosen.Add(chosen);
                // If we reach max choice count, disable all choice
                if (!CanChoose)
                {
                    SetChoiceChoosability(false);
                }
            }
            // Set confirm button if we chose 
            m_confirm.interactable = CanConfirm;
        }
        void OnUnchoose(IChoice chosen) 
        {
            if (m_chosen.Remove(chosen)) 
            {
                SetChoiceChoosability(true);
                m_confirm.interactable = CanConfirm;
            }
        }

        void SetChoiceChoosability(bool value) 
        {
            foreach (IChoice choice in m_currentContext.ChooseFrom)
            {
                choice.Choosable = value;
            }
        }
    }
}