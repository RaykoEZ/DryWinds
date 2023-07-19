using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.UI
{
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] Slider m_result = default;
        [SerializeField] float m_transitionDuration = default;
        [SerializeField] Image m_resultFill = default;
        [SerializeField] Gradient m_warningGradient = default;
        [SerializeField] AnimationCurve m_lerpSpeed = default;
        [SerializeField] bool m_smoothValueChange = default;
        public float Current => m_result.value;
        public float Max => m_result.maxValue;
        bool m_changeInProgress = false;
        float m_currentTargetVal = 0f;
        Coroutine m_currentResultTransition = default;
        public void SetBarValue(float val, bool forceInstantChange = false) 
        {
            if(val == m_result.value || val == m_currentTargetVal) 
            { 
                return; 
            }

            if (!m_smoothValueChange || forceInstantChange) 
            {
                m_result.value = val;
                return;
            }

            if (m_changeInProgress) 
            {
                StopCoroutine(m_currentResultTransition);
            }
            m_currentTargetVal = val;
            m_currentResultTransition = StartCoroutine(OnValueChange());
        }

        public void SetMaxValue(float val) 
        {
            if (val == m_result.maxValue) 
            { 
                return; 
            }
            m_result.maxValue = val;
        }

        IEnumerator OnValueChange() 
        {
            m_changeInProgress = true;
            float elapsedTime = 0f;
            while(elapsedTime < m_transitionDuration) 
            {
                elapsedTime += Time.smoothDeltaTime;
                m_result.value = Mathf.Lerp(m_result.value, m_currentTargetVal, m_lerpSpeed.Evaluate(elapsedTime / m_transitionDuration));
                m_resultFill.color = m_warningGradient.Evaluate(m_result.normalizedValue);
                yield return new WaitForEndOfFrame();
            }
            m_changeInProgress = false;
        }
    }
}


