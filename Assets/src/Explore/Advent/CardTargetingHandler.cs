using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
namespace Curry.Explore
{
    public class CardTargetingHandler: MonoBehaviour 
    {
        [SerializeField] LineRenderer m_line = default;
        public void BeginLine(Vector3 origin) 
        {
            m_line.positionCount = 2;
            m_line.SetPosition(0, origin);

        }
        public void UpdateLine(PointerEventData eventData)
        {
            Vector2 screen = Pointer.current.position.ReadValue();
            Vector2 world = Camera.main.ScreenToWorldPoint(screen);
            m_line.SetPosition(1, world);
        }
        public void Clear() 
        {
            m_line.positionCount = 0;
        }
    }

}