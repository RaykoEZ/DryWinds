using UnityEngine;
using UnityEngine.InputSystem;
namespace Curry.Explore
{
    public delegate void OnGuideFinish();
    // Display guide line when choosing targets with card effects
    public class TargetGuideHandler : MonoBehaviour
    {
        [SerializeField] LineRenderer m_line = default;
        public event OnGuideFinish OnFinish;
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
            // Hide guide when pointer clicked/position selected
            if (Mouse.current.rightButton.isPressed || !Mouse.current.leftButton.isPressed)
            {
                Clear();
                return;
            }
            Vector2 screen = Pointer.current.position.ReadValue();
            Vector2 world = Camera.main.ScreenToWorldPoint(screen);
            m_line.SetPosition(0, m_origin.position);
            m_line.SetPosition(1, world);
        }
        protected void Clear()
        {
            m_activated = false;
            m_line.positionCount = 0;
            OnFinish?.Invoke();
        }
    }

}