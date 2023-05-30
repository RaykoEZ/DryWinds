using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Curry.UI
{
    public class LightGradientHandler : MonoBehaviour
    {
        [SerializeField] Light2D m_light = default;
        [SerializeField] Gradient m_colourTransition = default;
        [SerializeField] float m_cycleLength = default;
        [SerializeField] float m_waitAfterCycle = default;
        delegate void CycleFinished();
        event CycleFinished OnFinish;
        float m_timer = 0f;
        // Use this for initialization
        void Awake()
        {
            OnFinish += Init;
        }
        void Start()
        {
            Init();
        }
        void Init() 
        {
            StartCoroutine(UpdateLightColour());
        }

        // update color of light
        IEnumerator UpdateLightColour() 
        {
            if (m_cycleLength == 0f) yield break;
            float t;
            while (m_timer < m_cycleLength) 
            {
                m_timer += Time.deltaTime;
                t = m_timer / m_cycleLength;
                Color newColour = m_colourTransition.Evaluate(t);
                m_light.color = newColour;
                m_light.intensity = newColour.a;
                yield return new WaitForEndOfFrame();
            }
            // Restart cycle
            m_timer = 0f;
            yield return new WaitForSeconds(m_waitAfterCycle);
            OnFinish?.Invoke();
            yield return null;
        }
    }
}