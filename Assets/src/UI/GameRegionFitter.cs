using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Util
{
    public class GameRegionFitter : MonoBehaviour
    {
        [SerializeField] Camera m_mainCamera = default;
        [SerializeField] BoxCollider2D m_screenRegion = default;
        [SerializeField] BoxCollider2D m_koRegion = default;
        [SerializeField] float m_koRegionSize = default;

        // Start is called before the first frame update
        void Start()
        {
            float aspect = (float)Screen.width / Screen.height;
            float orthoSize = m_mainCamera.orthographicSize;

            float width = 2f * orthoSize * aspect;
            float height = 2f * orthoSize;

            m_screenRegion.size = new Vector2(width, height);
            m_koRegion.size = m_koRegionSize * m_screenRegion.size;
        }
    }
}
