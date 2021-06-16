using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class DrawingCursor : MonoBehaviour
    {
        [SerializeField] Camera Camera = default;
        [SerializeField] GameObject LineStrokeObject = default;
        [SerializeField] Transform StrokeParent = default;

        protected StrokeManager m_strokeManager = new StrokeManager();
        protected LineStroke m_currentStroke = default;
        protected Vector2 m_previousPosition = default;

        protected bool strokeEnded = true;

        void Start()
        {
            m_strokeManager.Init(); 
        }

        // Update is called once per frame
        void Update()
        {
            Draw();
        }

        protected virtual void Draw()
        {
            // current stroke ended
            if (Input.GetMouseButtonDown(0))
            {
                strokeEnded = true;
                m_currentStroke = null;
            }

            Vector2 mousePosition = Camera.ScreenToWorldPoint(Input.mousePosition);
            // start a new stroke if we hold LMB and is moving
            if (Input.GetMouseButton(0) && mousePosition != m_previousPosition)
            {
                strokeEnded = false;
                if (strokeEnded || m_currentStroke == null) 
                {
                    // make new stroke, can be pooled objects in the future
                    GameObject newStroke = Instantiate(LineStrokeObject, StrokeParent);
                    m_currentStroke = newStroke.GetComponent<LineStroke>();
                }

                m_previousPosition = mousePosition;
                m_currentStroke.OnDraw(mousePosition);
            }
        }
    }
}
