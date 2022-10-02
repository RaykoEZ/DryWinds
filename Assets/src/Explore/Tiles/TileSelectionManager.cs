using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using Curry.UI;
using Curry.Events;

namespace Curry.Explore
{
    public class TileSelectionManager : MonoBehaviour
    {
        [SerializeField] GameObject m_highlightObject = default;
        [SerializeField] Tilemap m_map = default;
        [SerializeField] TileManager m_tileHighlightManager = default;
        [SerializeField] CameraManager m_cam = default;
        [SerializeField] CurryGameEventListener m_onTileSelect = default;
        public event OnTileSelect OnTileSelected = default;

        void Awake()
        {
            m_onTileSelect?.Init();
            m_tileHighlightManager.Clear();
            m_tileHighlightManager.Add(m_highlightObject, Vector3.zero, transform);
        }

        void HighlightTileInternal(Vector3 newPos)
        {
            m_tileHighlightManager.MoveTileTo(newPos);
            m_tileHighlightManager.Show();
        }

        public void HandleHighlightTile(EventInfo info)
        {
            if (info.Payload == null) 
            {
                return;
            }

            bool doesPosExist = info.Payload.TryGetValue("pressPosition", out object p); 
            bool buttonPressed = info.Payload.TryGetValue("button", out object b);
            bool valueCheck = doesPosExist && buttonPressed;
            if (valueCheck && 
                p is Vector2 pos && 
                b is PointerEventData.InputButton button && button == PointerEventData.InputButton.Left) 
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
                Vector3Int gridCoord = m_map.WorldToCell(worldPos);
                Vector3 centerWorld = m_map.GetCellCenterWorld(gridCoord);
                centerWorld.z = 0f;
                OnTileSelected?.Invoke(gridCoord);
                m_cam.FocusCamera(centerWorld);
                HighlightTileInternal(centerWorld);
            }

        }
    }
}