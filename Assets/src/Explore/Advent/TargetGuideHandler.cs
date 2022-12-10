using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
namespace Curry.Explore
{
    // Display guide line when choosing targets with card effects
    public class TargetGuideHandler: MonoBehaviour 
    {
        [SerializeField] LineRenderer m_line = default;
        bool m_activated = false;
        Transform m_origin;
        void FixedUpdate()
        {
            if (m_activated) 
            {
                UpdateLine();
            }
        }
        public void BeginLine(Transform origin) 
        {
            m_origin = origin;
            m_line.positionCount = 2;
            // Setting each point to prevent previous dirty points appearing before updating
            m_line.SetPosition(1, m_origin.position);
            m_line.SetPosition(0, m_origin.position);
            m_activated = true;
        }
        public void UpdateLine()
        {
            Vector2 screen = Pointer.current.position.ReadValue();
            Vector2 world = Camera.main.ScreenToWorldPoint(screen);
            m_line.SetPosition(0, m_origin.position);
            m_line.SetPosition(1, world);
        }
        public void Clear() 
        {
            m_activated = false;
            m_line.positionCount = 0;
        }
    }

}