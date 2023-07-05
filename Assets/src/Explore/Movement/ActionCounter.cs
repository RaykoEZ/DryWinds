using UnityEngine;
using TMPro;

namespace Curry.Explore
{
    public class ActionCounter : MonoBehaviour 
    {
        [SerializeField] int m_maxMoveCount = default;
        [SerializeField] protected TextMeshProUGUI m_moveCount = default;
        [SerializeField] protected TextMeshProUGUI m_diff = default;
        bool m_onPreview = false;
        int m_previous;
        int m_current = 0;
        public int CurrentActionCount => m_onPreview? m_previous : m_current;
        private void Start()
        {
            UpdateMoveCountDisplay(m_maxMoveCount);
        }
        public void UpdateMoveCountDisplay(int current)
        {
            m_previous = m_current;
            m_current = Mathf.Clamp(current, 0, m_maxMoveCount);
            m_moveCount.text = $"{m_current} / {m_maxMoveCount}";
            if (m_onPreview)
            {
                m_diff.text = "";
                m_onPreview = false;
            }
        }
        public void PreviewCost(int toSpend) 
        {
            if (!m_onPreview)
            {
                int newCurrent = Mathf.Clamp(m_current - toSpend, 0, m_maxMoveCount);
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
