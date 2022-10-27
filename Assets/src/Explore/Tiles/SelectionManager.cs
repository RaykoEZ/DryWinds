using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using Curry.UI;
using Curry.Events;

namespace Curry.Explore
{
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] GameObject m_previewTerrainTile = default;
        [SerializeField] GameObject m_previewRangeTile = default;
        [SerializeField] GameObject m_selectionTile = default;
        [SerializeField] Tilemap m_map = default;
        [SerializeField] TileManager m_tileHighlightManager = default;
        [SerializeField] CameraManager m_cam = default;
        [SerializeField] RangeDisplayHandler m_rangeDisplay = default;
        [SerializeField] CurryGameEventListener m_onTileSelect = default;

        public event OnTileSelect OnTileSelected = default;
        protected ObjectId m_previewTileId;

        void Awake()
        {
            m_onTileSelect?.Init();
            m_previewTileId = new ObjectId(m_previewTerrainTile);
            m_tileHighlightManager.Add(m_previewTerrainTile, Vector3.zero, transform);
        }

        void HighlightTileInternal(Vector3Int newCoord, bool focusCamera = true)
        {
            m_rangeDisplay?.HidePrompt();
            Vector3 centerWorld = m_map.GetCellCenterWorld(newCoord);
            centerWorld.z = 0f;
            if (focusCamera) 
            {
                m_cam.FocusCamera(centerWorld);
            }
            m_tileHighlightManager.MoveTileTo(m_previewTerrainTile, centerWorld);
            m_tileHighlightManager.Show(m_previewTileId);
        }

        public void CancelSelection() 
        {
            m_tileHighlightManager?.HideAll();
            m_rangeDisplay?.HidePrompt();
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

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(select.ClickScreenPosition);
            Vector3Int gridCoord = m_map.WorldToCell(worldPos);
            OnTileSelected?.Invoke(gridCoord);
            HighlightTileInternal(gridCoord);

            if (select.SelectedObject != null && 
                select.SelectedObject.TryGetComponent(out Adventurer player)) 
            {
                OnSelectPlayer(player, select.SelectionMode);
            }
        }

        void OnSelectPlayer(Adventurer player, TileSelectionMode mode) 
        {
            GameObject rangeTile;
            switch (mode)
            {
                case TileSelectionMode.Preview:
                    rangeTile = m_previewRangeTile;
                    break;
                case TileSelectionMode.Adventure:
                    rangeTile = m_selectionTile;
                    break;
                default:
                    rangeTile = m_previewRangeTile;
                    break;
            }
            m_rangeDisplay.ShowRange(
                    rangeTile,
                    player.Stats.ScoutRange,
                    player.transform.position,
                    player.transform);
        }
    }
}