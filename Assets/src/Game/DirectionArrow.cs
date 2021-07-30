using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Curry.Game
{
    [RequireComponent(typeof(LineRenderer))]
    public class DirectionArrow : MonoBehaviour
    {
        [SerializeField] Camera m_cam = default;
        [SerializeField] Transform m_origin = default;
        [SerializeField] LineRenderer m_lineRender = default;
        [SerializeField] float m_lengthScale = default;
        Vector2 m_mousePos = Vector2.zero;
        Vector3[] m_linePosition = {Vector3.zero, Vector3.zero};
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector2 mousePos = m_cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (m_linePosition[0] != m_origin.position ||
                m_mousePos != mousePos) 
            {
                m_mousePos = mousePos;
                Vector2 origin = m_origin.position;
                Vector2 dir = m_mousePos - origin;
                m_linePosition[0] = m_origin.position;
                m_linePosition[1] = origin + (dir * m_lengthScale);
                m_lineRender.SetPositions(m_linePosition);
            }
        }
    }
}
