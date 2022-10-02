using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Util;
namespace Curry.Explore
{
    public class RangeDisplayHandler : MonoBehaviour
    {
        [SerializeField] TileManager m_rangeTileManager = default;
        [SerializeField] Tilemap m_map = default;
        [SerializeField] RangeMapDatabase m_rangeDb = default;
        #region Methods to cancel prompt display
        public void HidePrompt()
        {
            m_rangeTileManager.Hide();
        }
        #endregion

        #region Show method used for spawning range tiles
        void Show(RangeMap tileOffsets, Vector3 origin, GameObject tileRef, Transform parent)
        {
            if (m_rangeTileManager.IsEmpty)
            {
                // If tiles never existed, make new tiles
                Vector3Int originCoord = m_map.LocalToCell(origin);
                // This is for showing/creating range tiles.
                foreach (Vector3Int p in tileOffsets?.OffsetsFromOrigin)
                {
                    Vector3 offsetPos = m_map.GetCellCenterLocal(originCoord + p);
                    m_rangeTileManager.Add(tileRef, offsetPos, parent);
                }
            }

            m_rangeTileManager.Show();

        }
        #endregion

        #region Show/ToggleRangeMap variants:

        public void ShowRange(
            GameObject tileToSpawn,
            int range,
            Vector3 origin,
            Transform parent,
            bool toggle = false)
        {
            RangeMap map = m_rangeDb.GetSquareRadiusMap(range);
            if (toggle)
            {
                Toggle_Internal(
                    origin,
                    map,
                    tileToSpawn,
                    parent);
            }
            else
            {
                Show_Internal(
                    origin,
                    map,
                    tileToSpawn,
                    parent);
            }
        }

        #endregion

        #region Utility for Show/ToggleRangeMap
        void Show_Internal(
            Vector3 origin,
            RangeMap rangeMap,
            GameObject tileRef,
            Transform parent)
        {
            m_rangeTileManager.Hide();
            Show(rangeMap, origin, tileRef, parent);
        }

        // true - Display is now active
        // false - Display is now not active
        bool Toggle_Internal(
            Vector3 origin,
            RangeMap rangeMap,
            GameObject tileRef,
            Transform parent)
        {

            bool tilesActive = m_rangeTileManager.IsActive;
            if (tilesActive)
            {
                HidePrompt();
                return !tilesActive;
            }
            m_rangeTileManager.Hide();

            Show(rangeMap, origin, tileRef, parent);
            return true;
        }
        #endregion
    }

}
