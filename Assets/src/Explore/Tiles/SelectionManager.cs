using Curry.Events;
using Curry.UI;
using Curry.Util;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Curry.Explore
{
    // For displaying tiles a player selects/previews OR a selection prompt.  
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] GameObject m_previewTerrainTile = default;
        [SerializeField] GameObject m_previewRangeTile = default;
        [SerializeField] GameObject m_selectionTile = default;
        [SerializeField] GameObject m_tileDropRef = default;

        [SerializeField] Tilemap m_map = default;
        [SerializeField] TargetGuideHandler m_targetGuide = default;
        [SerializeField] TileManager m_tileHighlightManager = default;
        [SerializeField] CameraManager m_cam = default;
        [SerializeField] RangeDisplayHandler m_rangeDisplay = default;
        [SerializeField] CurryGameEventListener m_onAdventurePrompt = default;
        [SerializeField] CurryGameEventListener m_onCardActivate = default;

        public event OnTileSelect OnTileSelected = default;
        protected ObjectId m_previewTileId;

        void Start()
        {
            m_onAdventurePrompt?.Init();
            m_onCardActivate?.Init();
            m_targetGuide.OnFinish += CancelSelection;
            m_previewTileId = new ObjectId(m_previewTerrainTile);
            m_tileHighlightManager.Add(new ObjectId(m_previewTerrainTile), m_previewTerrainTile, Vector3.zero, transform);
        }
        public void TargetGuide(Transform origin) 
        {
            m_targetGuide?.BeginLine(origin);
        }
        public void EnableSelection()
        {
            m_onAdventurePrompt?.Init();
        }
        public void DisableSelection()
        {
            CancelSelection();
            m_onAdventurePrompt?.Shutdown();
        }
        public void CancelSelection()
        {
            m_tileHighlightManager?.HideAll();
            m_rangeDisplay?.HidePrompt();
        }

        public void SelectDropZoneTile(string dropName, RangeMap range, Transform parent)
        {
            CancelSelection();
            m_rangeDisplay.ShowRange(dropName, m_tileDropRef, range, parent);
        }

        public void OnSelectTile(EventInfo info)
        {
            if (info == null)
            {
                return;
            }
            TileSelectionInfo select = info as TileSelectionInfo;
            if (select == null)
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
                    player.MoveRange,
                    player.transform);
        }
    }
}