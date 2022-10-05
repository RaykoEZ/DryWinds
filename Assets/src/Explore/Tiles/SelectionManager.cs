using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using Curry.UI;
using Curry.Events;

namespace Curry.Explore
{
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] GameObject m_previewTile = default;
        [SerializeField] GameObject m_selectionTile = default;
        [SerializeField] PointerEventData.InputButton m_defaultSelectButton = default;
        [SerializeField] Tilemap m_map = default;
        [SerializeField] TileManager m_tileHighlightManager = default;
        [SerializeField] CameraManager m_cam = default;
        [SerializeField] RangeDisplayHandler m_rangeDisplay = default;
        [SerializeField] CurryGameEventListener m_onTileSelect = default;

        public event OnTileSelect OnTileSelected = default;

        void Awake()
        {
            m_onTileSelect?.Init();
            m_tileHighlightManager.Clear();
            m_tileHighlightManager.Add(m_previewTile, Vector3.zero, transform);
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
            if (info == null) 
            {
                return;
            }
            TileSelectionInfo select = info as TileSelectionInfo;
            if(select == null) 
            { 
                return; 
            }

            if (select.Button == m_defaultSelectButton) 
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(select.ClickScreenPosition);
                Vector3Int gridCoord = m_map.WorldToCell(worldPos);
                OnTileSelected?.Invoke(gridCoord);
                HighlightTileInternal(gridCoord);
            }

            if (select.SelectedObject != null && 
                select.SelectedObject.TryGetComponent(out Adventurer player)) 
            {
                GameObject rangeTile = 
                    select.SelectionMode == TileSelectionMode.Preview? 
                    m_previewTile : m_selectionTile;

                m_rangeDisplay.ShowRange(
                    rangeTile, 
                    player.ScoutRange,
                    player.transform.position, 
                    transform);
            }
        }
    }
}