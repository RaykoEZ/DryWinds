using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Curry.Skill;
using Curry.Game;

namespace Curry.Explore
{
    public class DrawingManager : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] Camera m_cam = default;
        [SerializeField] Explorer m_master = default;
        [SerializeField] PathMaker m_pathMaker = default;
        bool m_drawing = false;

        void Awake() 
        {
            m_pathMaker.Init(m_master);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 view = m_cam.ScreenToViewportPoint(mousePos);
            bool isOutside = view.x < 0f || view.x > 1f || view.y < 0f || view.y > 1f;
            if (m_drawing && Mouse.current.leftButton.isPressed && !isOutside)
            {
                Vector2 pos = m_cam.ScreenToWorldPoint(mousePos);
                MakePath(pos);
            }
        }

        public virtual void MakePath(Vector2 target)
        {
            VectorInput input = new VectorInput(target);
            m_pathMaker.OnEnter(input);
        }

        #region drawing brush
        public void OnPathFinish(InputAction.CallbackContext c)
        {
            if (c.interaction is PressInteraction)
            {
                switch (c.phase)
                {
                    case InputActionPhase.Performed:
                        // Finished a brush stroke
                        m_pathMaker.Interrupt();
                        m_drawing = false;
                        m_master.SetPath(m_pathMaker.CurrentPath);
                        m_master.StartExploration();
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_drawing = true;
        }
        #endregion
    }
}