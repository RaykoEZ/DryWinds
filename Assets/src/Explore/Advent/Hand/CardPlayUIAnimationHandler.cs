using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Explore
{
    // A utility class to prevent UI displays from flickering due to pointer alternating transitions
    // at edges of UI triggers 
    public class UITransitionBuffer 
    {
        const float c_transitionBufferTime = 0.2f;
        bool transitionBuffering = false;
        public bool Buffering => transitionBuffering;
        public IEnumerator Buffer()
        {
            transitionBuffering = true;
            yield return new WaitForSeconds(c_transitionBufferTime);
            transitionBuffering = false;
        }
    }

    // When pointer is in range, show ui, hide if not
    public class CardPlayUIAnimationHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] HideableUITrigger m_hidingUI = default;
        UITransitionBuffer m_buffer = new UITransitionBuffer();
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Buffer transition to stop unwanted flickers on edges
            if (!m_buffer.Buffering) 
            {
                m_hidingUI.Show();
                StartCoroutine(m_buffer.Buffer());
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            m_hidingUI.Hide();   
        }
    }
}