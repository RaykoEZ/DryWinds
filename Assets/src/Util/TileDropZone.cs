using UnityEngine;
using UnityEngine.EventSystems;
using Curry.Events;
namespace Curry.Explore
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TileDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Color m_onActive = default;
        [SerializeField] protected Color m_onPointerEnter = default;
        [SerializeField] protected SpriteRenderer m_sprite = default;
        [SerializeField] protected CurryGameEventTrigger m_onDrop = default;

        void OnEnable() 
        {
            m_sprite.color = m_onActive;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_sprite.color = m_onPointerEnter;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_sprite.color = m_onActive;
        }

        public void OnDrop(PointerEventData eventData)
        {
            PositionInfo info = new PositionInfo(transform.position);
            m_onDrop.TriggerEvent(info);
        }
    }
}