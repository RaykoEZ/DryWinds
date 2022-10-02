using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using Curry.UI;
using Curry.Events;

namespace Curry.Explore
{
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] GameObject m_highlightObject = default;
        [SerializeField] PointerEventData.InputButton m_defaultSelectButton = default;
        [SerializeField] Tilemap m_map = default;
        [SerializeField] TileManager m_tileHighlightManager = default;
        [SerializeField] CameraManager m_cam = default;
        [SerializeField] RangeDisplayHandler m_rangeDisplay = default;

        [SerializeField] CurryGameEventListener m_onTileSelect = default;
        [SerializeField] CurryGameEventListener m_onPlayerSelect = default;

        public event OnTileSelect OnTileSelected = default;

        void Awake()
        {
            m_onTileSelect?.Init();
            m_onPlayerSelect?.Init();
            m_tileHighlightManager.Clear();
            m_tileHighlightManager.Add(m_highlightObject, Vector3.zero, transform);
        }

        void HighlightTileInternal(Vector3Int newCoord, bool focusCamera = true)
        {
            m_rangeDisplay.HidePrompt();
            Vector3 centerWorld = m_map.GetCellCenterWorld(newCoord);
            centerWorld.z = 0f;
            if (focusCamera) 
            {
                m_cam.FocusCamera(centerWorld);
            }
            m_tileHighlightManager.MoveTileTo(centerWorld);
            m_tileHighlightManager.Show();
        }
        public void OnSelectTile(EventInfo info)
        {
            if (info.Payload == null) 
            {
                return;
            }
            bool buttonPressed = info.Payload.TryGetValue("button", out object b);
            bool doesPosExist = info.Payload.TryGetValue("pressPosition", out object p); 
            bool valueCheck = doesPosExist && buttonPressed;
            if (b is PointerEventData.InputButton button && button == m_defaultSelectButton && 
                valueCheck && p is Vector2 pos) 
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
                Vector3Int gridCoord = m_map.WorldToCell(worldPos);
                OnTileSelected?.Invoke(gridCoord);
                HighlightTileInternal(gridCoord);
            }

        }

        public void OnSelectPlayer(EventInfo info) 
        {
            if (info.Payload == null)
            {
                return;
            }
            bool buttonPressed = info.Payload.TryGetValue("button", out object b);
            bool doesPlayerExist = info.Payload.TryGetValue("clickObject", out object p);

            if(b is PointerEventData.InputButton button && button == m_defaultSelectButton && 
                doesPlayerExist && p is GameObject go && go.TryGetComponent(out Adventurer player)) 
            {
                Vector3 worldPos = go.transform.position;
                Vector3Int gridCoord = m_map.WorldToCell(worldPos);
                HighlightTileInternal(gridCoord);
                m_rangeDisplay.ShowRange(m_highlightObject, player.ScoutRange, go.transform.position, transform);
            }
        }
    }
}