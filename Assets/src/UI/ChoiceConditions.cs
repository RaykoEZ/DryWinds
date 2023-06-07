using System;
using System.Collections.Generic;
using UnityEngine;

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
        public List<IChoice> ChooseFrom
        {
            get { return m_availableChoices; }
            set { m_availableChoices = value; }
        }
        public ChoiceContext(ChoiceConditions conditions, List<IChoice> availableChoices)
        {
            m_conditions = conditions;
            m_availableChoices = availableChoices;
        }
    }
    public struct ChoiceResult
    {
        public ChoiceStatus Status { get; set; }
        public IReadOnlyList<IChoice> Chosen { get; set; }
        public IReadOnlyList<IChoice> ChoseFrom { get; set; }
        public ChoiceResult(ChoiceStatus status, List<IChoice> choices, List<IChoice> chosen)
        {
            Status = status;
            Chosen = chosen;
            ChoseFrom = choices;
        }
        public enum ChoiceStatus
        {
            Confirmed,
            Cancelled
        }
    }
    #endregion
}