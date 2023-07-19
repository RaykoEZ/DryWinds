using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
using System.Runtime.InteropServices.ComTypes;

namespace Curry.Explore
{
    public class FogOfWar : MonoBehaviour
    {
        [SerializeField] Tilemap m_map = default;
        [SerializeField] TileBase m_clearTile = default;
        [SerializeField] TileBase m_fogTile = default;
        [SerializeField] CurryGameEventListener m_onTileReveal = default;
        [SerializeField] CurryGameEventListener m_onTileFog = default;
        void Awake()
        {
            m_onTileReveal?.Init();
            m_onTileFog?.Init();
        }
        public void OnEnemyReveal(ICharacter reveal)
        {
            SetFogOfWar(reveal.WorldPosition, clearFog: true);
        }
        public void OnEnemyHide(ICharacter hide)
        {
            SetFogOfWar(hide.WorldPosition, clearFog: false);
        }
        public void OnMovementBlocked(Vector3 pos)
        {
            SetFogOfWar(pos);
        }
        public void SetFogOfWar(Vector3 worldPos, bool clearFog = true) 
        {
            Vector3Int mapCoord = m_map.WorldToCell(worldPos);
            TileBase tileToSet = clearFog ? m_clearTile : m_fogTile;
            if (m_map.GetTile(mapCoord) == tileToSet) 
            {
                return;
            }
            m_map.SetTile(mapCoord, tileToSet);
        }
        public bool IsCellClear(Vector3Int cell) 
        {
            TileBase tile = m_map.GetTile(cell);
            bool ret = tile == null || tile == m_clearTile;
            return ret;
        }

        public void FogTile(EventInfo info) 
        {
            if (info == null) return;

            if (info is TileSelectionInfo select)
            {
                SetFogOfWar(select.ClickWorldPos, clearFog: false);
            }
            else if(info is CharacterInfo player) 
            {
                SetFogOfWar(player.Character.WorldPosition, clearFog: false);
            }
        }

        public void RevealTile(EventInfo info) 
        {
            if (info == null) return;

            if (info is TileSelectionInfo select)
            {
                SetFogOfWar(select.ClickWorldPos);
            }
            else if (info is CharacterInfo player)
            {
                SetFogOfWar(player.Character.WorldPosition);
            }
            else if (info is RangeInfo range) 
            { 
                foreach(Vector3 pos in range.WorldPositions) 
                {
                    SetFogOfWar(pos);
                }
            }
        }
    }
}