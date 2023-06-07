using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    public class PositionConfirmDisplay : MonoBehaviour
    {
        [SerializeField] SpriteRenderer m_display = default;
        [SerializeField] Collider2D m_collider = default;
        [SerializeField] CurryGameEventListener m_onPositionConfirm = default;
        [SerializeField] Color m_onHighlightColour = default;
        void Start()
        {
            m_onPositionConfirm?.Init();
        }
        public void OnChoosePosition(EventInfo info)
        {
            if(info is PositionInfo pos) 
            {
                Show(pos.WorldPosition);
            }
        }
        public void Show(Vector3 worldPosition)
        {
            transform.position = worldPosition;
            m_display.color = m_onHighlightColour;
            m_collider.enabled = true;
        }
        public void Hide() 
        {
            m_display.color = Color.clear;
            m_collider.enabled = false;
        }
        public void OnConfrmPosition() 
        {
            Hide();
        }
    }
}