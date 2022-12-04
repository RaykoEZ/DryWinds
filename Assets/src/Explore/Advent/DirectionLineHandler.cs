using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Explore
{
    public class DirectionLineHandler: MonoBehaviour 
    {
        [SerializeField] LineRenderer m_line = default;
        public void BeginLine(Transform origin) 
        {
            m_line.positionCount = 2;
            m_line.SetPosition(0, origin.position);

        }
        public void UpdateLine(PointerEventData eventData)
        {
            m_line.SetPosition(1, eventData.position);
        }
        public void Clear() 
        {
            m_line.positionCount = 0;
        }
    }

}