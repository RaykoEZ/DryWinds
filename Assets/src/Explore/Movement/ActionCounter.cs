using UnityEngine;
using TMPro;
using Curry.UI;

namespace Curry.Explore
{
    public class ActionCounter : MonoBehaviour 
    {
        [SerializeField] int m_maxMovePoint = default;
        [SerializeField] protected TextMeshProUGUI m_apValueField = default;
        [SerializeField] protected TextMeshProUGUI m_diff = default;
        [SerializeField] ResourceBar m_apChargeBar = default;
        bool m_onPreview = false;
        int m_previous;
        int m_current = 0;
        public int CurrentActionPoint => m_onPreview? m_previous : m_current;
        void Start()
        {
            m_apChargeBar.OnFinish += RecoverAp;
            m_apChargeBar.SetMaxValue(m_maxMovePoint);
            m_apChargeBar.SetBarValue(0f, true);
            UpdateMoveCountDisplay(m_maxMovePoint);
        }
        public void RecoverAp() 
        {
            m_apChargeBar.SetBarValue(0f, true);
            UpdateMoveCountDisplay(m_current + 1);
        }
        public void Empty() 
        {
            UpdateMoveCountDisplay(0);
        }
        public void UpdateMoveCountDisplay(int current, bool preview = false)
        {
            m_previous = m_current;
            m_current = Mathf.Clamp(current, 0, m_maxMovePoint);
            m_apValueField.text = $"{m_current} / {m_maxMovePoint}";
            if (m_onPreview)
            {
                m_diff.text = "";
                m_onPreview = false;
            }
            // Charge Ap until Ap reaches maximum value
            if (m_current < m_maxMovePoint && !preview) 
            {
                StartChargingAp();
            }
        }
        public void StartChargingAp() 
        {
            if(CurrentActionPoint < m_maxMovePoint) 
            {
                m_apChargeBar.SetBarValue(m_apChargeBar.Max);
            }
        }
        public void PauseApCharging() 
        {
            m_apChargeBar.StopAllCoroutines();
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
                UpdateMoveCountDisplay(newCurrent, true);
                m_onPreview = true;
            }
        }
        public void CancelPreview() 
        {
            if (m_onPreview)
            {
                UpdateMoveCountDisplay(m_previous, true);
            }
        }
    }
}
