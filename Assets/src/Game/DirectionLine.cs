using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class DirectionLine : MonoBehaviour
    {
        [SerializeField] Camera m_camera = default;
        [SerializeField] LineRenderer m_indicatorLine = default;
        [SerializeField] float m_lengthScale = default;
        Vector2 m_mouseDirection = Vector2.up;
        Vector3[] m_indicatorPoints = new Vector3[2];

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector2 mousePosition = m_camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 ownPos = gameObject.transform.position;
            Vector2 dir =  mousePosition - ownPos;
            dir.Normalize();
            if(m_mouseDirection != dir) 
            {
                m_mouseDirection = dir;
                m_indicatorPoints[0] = ownPos;
                m_indicatorPoints[1] = ownPos + (dir * m_lengthScale);
                m_indicatorLine.SetPositions(m_indicatorPoints);
            }
        }
    }
}
