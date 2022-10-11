using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;
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

        void SetFogOfWar(Vector3 worldPos, bool clearFog = true) 
        {
            Vector3Int mapCoord = m_map.WorldToCell(worldPos);
            TileBase tileToSet = clearFog ? m_clearTile : m_fogTile;
            m_map.SetTile(mapCoord, tileToSet);
        }

        public void FogTile(EventInfo info) 
        {
            if (info == null) return;

            if (info is TileSelectionInfo select)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(select.ClickScreenPosition);
                SetFogOfWar(worldPos, clearFog: false);
            }
            else if(info is PlayerInfo player) 
            {
                SetFogOfWar(player.PlayerStats.WorldPosition, clearFog: false);
            }
        }

        public void RevealTile(EventInfo info) 
        {
            if (info == null) return;

            if (info is TileSelectionInfo select)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(select.ClickScreenPosition);
                SetFogOfWar(worldPos);
            }
            else if (info is PlayerInfo player)
            {
                SetFogOfWar(player.PlayerStats.WorldPosition);
            }
        }
    }
}