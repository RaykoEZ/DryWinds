using UnityEngine;
using TMPro;

namespace Curry.Explore
{
    public class ActionCounter : MonoBehaviour 
    {
        [SerializeField] int m_maxMovePoint = default;
        [SerializeField] protected TextMeshProUGUI m_apValueField = default;
        [SerializeField] protected TextMeshProUGUI m_diff = default;
        [SerializeField] TurnEnd m_turnEnd = default;
        bool m_onPreview = false;
        int m_previous;
        int m_current = 0;
        public int CurrentActionPoint => m_onPreview? m_previous : m_current;
        void Start()
        {
            m_turnEnd.OnTurnEnding += FullRecovery;
            UpdateMoveCountDisplay(m_maxMovePoint);
        }
        public void FullRecovery() 
        {
            UpdateMoveCountDisplay(m_maxMovePoint);
        }
        public void Empty() 
        {
            UpdateMoveCountDisplay(0);
        }
        public void UpdateMoveCountDisplay(int current)
        {
            m_previous = m_current;
            m_current = Mathf.Clamp(current, 0, m_maxMovePoint);
            m_apValueField.text = $"{m_current} / {m_maxMovePoint}";
            if (m_onPreview)
            {
                m_diff.text = "";
                m_onPreview = false;
            }
        }
        public void PreviewFullRecovery()
        {
            PreviewCost(-m_maxMovePoint);
        }
        public void PreviewFullSpend()
        {
            PreviewCost(m_maxMovePoint);
        }
        public void PreviewCost(int toSpend) 
        {
            if (!m_onPreview)
            {
                int newCurrent = Mathf.Clamp(m_current - toSpend, 0, m_maxMovePoint);
                string display;
                if (toSpend > 0) 
                {
                    display = $"<color=red>( - {toSpend})</color>";
                }else if(newCurrent == m_current) 
                {
                    display = "<color=white>( + 0)</color>";
                }
                else 
                {
                    display = $"<color=green>( + {-toSpend})</color>";
                }
                m_diff.text = display;
                UpdateMoveCountDisplay(newCurrent);
                m_onPreview = true;
            }
        }
        public void CancelPreview() 
        {
            if (m_onPreview)
            {
                UpdateMoveCountDisplay(m_previous);
            }
        }
    }
}
