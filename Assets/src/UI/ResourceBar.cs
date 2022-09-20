using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.UI
{
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] Slider m_slider = default;
        [SerializeField] float m_transitionDuration = default;
        [SerializeField] Image m_fill = default;
        [SerializeField] Gradient m_warningGradient = default;
        [SerializeField] bool m_smoothValueChange = default;
        
        bool m_changeInProgress = false;
        float m_currentTargetVal = 0f;
        Coroutine m_currentTransition = default;

        public void SetBarValue(float val, bool forceInstantChange = false) 
        {
            if(val == m_slider.value || val == m_currentTargetVal) 
            { 
                return; 
            }

            if (!m_smoothValueChange || forceInstantChange) 
            {
                m_slider.value = val;
                return;
            }

            if (m_changeInProgress) 
            {
                StopCoroutine(m_currentTransition);
            }

            m_currentTargetVal = val;
            m_currentTransition = StartCoroutine(OnValueChange());
        }

        public void SetMaxValue(float val) 
        {
            if (val == m_slider.maxValue) 
            { 
                return; 
            }
            m_slider.maxValue = val;
        }

        IEnumerator OnValueChange() 
        {
            m_changeInProgress = true;
            float elapsedTime = 0f;
            while(elapsedTime < m_transitionDuration) 
            {
                m_slider.value = Mathf.Lerp(m_slider.value, m_currentTargetVal, elapsedTime / m_transitionDuration);
                m_fill.color = m_warningGradient.Evaluate(m_slider.normalizedValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            m_changeInProgress = false;
        }

    }

}


