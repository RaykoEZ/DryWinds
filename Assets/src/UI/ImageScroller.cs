using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.UI
{
    public class ImageScroller : MonoBehaviour
    {
        [SerializeField] RawImage m_image = default;
        [SerializeField] Vector2 m_scrollDelta = default;

        // Update is called once per frame
        void Update()
        {
            m_image.uvRect = new Rect(m_image.uvRect.position + m_scrollDelta * Time.deltaTime, m_image.uvRect.size);
        }
    }
}