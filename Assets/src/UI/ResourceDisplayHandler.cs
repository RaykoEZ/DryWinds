using System;
using System.Collections;
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
            if (!m_onPreview) 
            {
                SetDiffValue(val);
            }
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
                m_onPreview = true;
                SetDiffValue(m_current.Current, true);
                // preview bar shows behind current bar, showing the difference of old vs new
                SetCurrentValue(val);
            }
        }
        public void TryCancelPreview() 
        {
            if (m_onPreview)
            {
                m_onPreview = false;
                SetCurrentValue(m_previousValue);
            }
        }
        void SetDiffValue(float val, bool instant = false)
        {
            m_diff?.SetBarValue(val, instant);
        }
    }
}


