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
            if (m_onPreview) 
            {
                ConfirmPreview();
            }
        }
        public void SetMaxValue(float val) 
        {
            m_current?.SetMaxValue(val);
            m_diff?.SetMaxValue(val);
        }
        public void Preview(float newVal, out Action confirm, out Action cancel) 
        {
            // preview bar shows behind current bar, showing the difference of old vs new
            SetCurrentValue(newVal);
            m_diff?.SetBarValue(m_previousValue);
            m_onPreview = true;
            confirm = ConfirmPreview;
            cancel = CancelPreview;
        }
        void ConfirmPreview() 
        {
            m_diff?.SetBarValue(m_current.Current, true);
            m_onPreview = false;
        }
        void CancelPreview() 
        {
            m_onPreview = false;
            SetCurrentValue(m_previousValue);
        }
    }
}


