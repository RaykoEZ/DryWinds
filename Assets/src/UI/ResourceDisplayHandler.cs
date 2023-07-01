using System;
using UnityEngine;

namespace Curry.UI
{
    // Adapts resource bar display calls, handles bar value change preview
    public class ResourceDisplayHandler : MonoBehaviour
    {
        [SerializeField] ResourceBar m_current = default;
        [SerializeField] ResourceBar m_diff = default;
        float m_previousValue;
        bool m_onPreview = false;
        public void SetCurrentValue(float val, bool instant = false) 
        {
            m_previousValue = m_current.Current;
            m_current?.SetBarValue(val, instant);
            TryConfirmPreview(val);
        }
        public void SetMaxValue(float val) 
        {
            m_current?.SetMaxValue(val);
            m_diff?.SetMaxValue(val);
        }
        public void Preview(float val) 
        {
            if (!m_onPreview)
            {
                // preview bar shows behind current bar, showing the difference of old vs new
                SetCurrentValue(val);
                m_diff?.SetBarValue(m_previousValue, true);
                m_onPreview = true;
            }
        }
        void TryConfirmPreview(float val) 
        {
            if (m_onPreview) 
            {
                m_diff?.SetBarValue(val);
                m_onPreview = false;
            }
        }
        public void TryCancelPreview() 
        {
            if (m_onPreview)
            {
                SetCurrentValue(m_previousValue);
            }
        }
    }
}


