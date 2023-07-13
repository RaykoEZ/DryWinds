using Curry.Events;
using Curry.UI;
using Curry.Util;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Curry.Explore
{
    public delegate void OnSelectionCancel();
    // For displaying tiles a player selects/previews OR a selection prompt.  
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] GameObject m_previewTerrainTile = default;
        [SerializeField] GameObject m_previewRangeTile = default;
        [SerializeField] GameObject m_selectionTile = default;
        [SerializeField] GameObject m_tileDropRef = default;

        [SerializeField] FogOfWar m_fogOfWar = default;
        [SerializeField] Tilemap m_terrain = default;
        [SerializeField] TargetGuideHandler m_targetGuide = default;
        [SerializeField] TileManager m_tileHighlightManager = default;
        [SerializeField] CharacterDetailDisplay m_playerDetail = default;
        [SerializeField] CharacterDetailDisplay m_enemyDetail = default;
        [SerializeField] CameraManager m_cam = default;
        [SerializeField] RangeDisplayHandler m_rangeDisplay = default;
        [SerializeField] CurryGameEventListener m_onAdventurePrompt = default;
        [SerializeField] CurryGameEventListener m_onCardActivate = default;
        public event OnTileSelect OnTileSelected = default;
        public event OnSelectionCancel OnCancel = default;
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
            if (info != null && info is TileSelectionInfo select)
            {
                OnSelectTile(select);
            }
        }
        public void OnSelectTile(TileSelectionInfo info) 
        {
            if (info == null)
            {
                return;
            }
            Vector3Int gridCoord = m_terrain.WorldToCell(info.ClickWorldPos);
            OnTileSelected?.Invoke(gridCoord);
            HighlightTileInternal(gridCoord);
            bool isCoordClear = m_fogOfWar.IsCellClear(gridCoord);
            bool hasCharacter = GameUtil.TrySearchTarget(
                info.ClickWorldPos,
                LayerMask.GetMask(MovementManager.s_gameplayCollisionFilters),
                out TacticalCharacter result);
            if (isCoordClear && hasCharacter && result != null)
            {
                OnSelectCharacter(result, info.SelectionMode);
            }
            else
            {
                m_playerDetail?.EndDisplay();
                m_enemyDetail?.EndDisplay();
            }
        }
        void OnSelectCharacter(TacticalCharacter character, TileSelectionMode mode) 
        {
            if(character is IPlayer) 
            {
                OnSelectPlayer(character, mode);
            }
            else 
            {
                m_enemyDetail?.Display(character);
            }
        }
        void HighlightTileInternal(Vector3Int newCoord, bool focusCamera = true)
        {
            m_rangeDisplay?.HidePrompt();
            OnCancel?.Invoke();
            Vector3 centerWorld = m_terrain.GetCellCenterWorld(newCoord);
            centerWorld.z = 0f;
            if (focusCamera)
            {
                m_cam.FocusCamera(centerWorld);
            }
            m_tileHighlightManager.MoveTileTo(new ObjectId(m_previewTerrainTile), centerWorld);
            m_tileHighlightManager.Show(m_previewTileId);
        }
        void OnSelectPlayer(TacticalCharacter player, TileSelectionMode mode)
        {
            GameObject rangeTile;
            switch (mode)
            {
                case TileSelectionMode.Preview:
                    rangeTile = m_previewRangeTile;
                    m_playerDetail?.Display(player);
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
                    player.GetTransform());
        }
    }
}