using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.UI
{
    public delegate void OnChargeFinish();
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] protected Slider m_result = default;
        [SerializeField] protected float m_transitionDuration = default;
        [SerializeField] protected Image m_resultFill = default;
        [SerializeField] protected Gradient m_warningGradient = default;
        [SerializeField] protected AnimationCurve m_lerpSpeed = default;
        [SerializeField] protected bool m_smoothValueChange = default;
        public event OnChargeFinish OnFinish;
        public float Current => m_result.value;
        public float Max => m_result.maxValue;
        bool m_changeInProgress = false;
        protected float m_currentTargetVal = 0f;
        Coroutine m_currentResultTransition = default;
        public void SetBarValue(float val, bool forceInstantChange = false) 
        {
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
            m_result.maxValue = val;
        }
        protected virtual IEnumerator OnValueChange() 
        {
            m_changeInProgress = true;
            float elapsedTime = 0f;
            float t = 0f;
            float start = m_result.value;
            while (elapsedTime < m_transitionDuration) 
            {
                m_result.value = Mathf.Lerp(start, m_currentTargetVal, m_lerpSpeed.Evaluate(t));
                m_resultFill.color = m_warningGradient.Evaluate(m_result.normalizedValue);
                elapsedTime += Time.smoothDeltaTime;
                t = elapsedTime / m_transitionDuration;
                yield return new WaitForEndOfFrame();
            }
            m_changeInProgress = false;
            OnFinish?.Invoke();
        }
    }
}


