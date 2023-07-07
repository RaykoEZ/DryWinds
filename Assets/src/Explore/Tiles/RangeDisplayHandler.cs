using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Util;
namespace Curry.Explore
{
    public class RangeDisplayHandler : MonoBehaviour
    {
        [SerializeField] TileManager m_rangeTileManager = default;
        [SerializeField] Tilemap m_map = default;
        [SerializeField] RadialRangeMapGenerator m_rangeDb = default;
        [SerializeField] Transform m_defaultParent = default;
        ObjectId m_currentObjectId;
        #region Methods to cancel prompt display
        public void HidePrompt()
        {
            m_rangeTileManager.HideAll();
        }
        #endregion
        // make new id for each unique range pattern
        ObjectId MakeRangeMapId(int range, GameObject tileRef) 
        {
            string idVal = "r" + range;
            idVal += tileRef.name;
            return new ObjectId(idVal);
        }

        #region Show method used for spawning range tiles
        void Show(ObjectId id, RangeMap tileOffsets, GameObject tileRef, Transform parent)
        {
            if (tileRef == null)
            { 
                return; 
            }
            if (!m_rangeTileManager.DoTilesExist(id))
            {
                MakeNewTiles(id, tileOffsets, tileRef, parent);

            }
            m_currentObjectId = id;
            m_rangeTileManager.Show(m_currentObjectId);
        }

        void MakeNewTiles(ObjectId id, RangeMap tileOffsets, GameObject tileRef, Transform parent) 
        {
            // If tiles never existed, make new tiles
            // This is for showing/creating range tiles.
            foreach (Vector3Int p in tileOffsets?.OffsetsFromOrigin)
            {
                Vector3 offsetPos = m_map.CellToWorld(p);
                m_rangeTileManager.Add(id, tileRef, offsetPos, parent);
            }
        }
        #endregion

        #region Show/ToggleRangeMap variants:

        public void ShowRange(
            GameObject tileToSpawn,
            int range,
            Transform parent,
            bool toggle = false)
        {
            RangeMap map = m_rangeDb.GetSquareRadiusMap(range);
            ObjectId id = MakeRangeMapId(range, tileToSpawn);
            if (toggle)
            {
                Toggle_Internal(
                    id,
                    map,
                    tileToSpawn,
                    parent);
            }
            else
            {
                Show_Internal(
                    id,
                    map,
                    tileToSpawn,
                    parent);
            }
        }

        public void ShowRange(
            string name,
            GameObject tileToSpawn,
            RangeMap range,
            Transform parent = null,
            bool toggle = false)
        {
            Transform parentToUse = parent == null ? m_defaultParent : parent;
            ObjectId id = new ObjectId(name + tileToSpawn.name);
            if (toggle)
            {
                Toggle_Internal(
                    id,
                    range,
                    tileToSpawn,
                    parentToUse);
            }
            else
            {
                Show_Internal(
                    id,
                    range,
                    tileToSpawn,
                    parentToUse);
            }
        }
        public void MoveRangeTile(string nameOfTileItem, GameObject tileRef, Vector3 offset) 
        {
            ObjectId id = new ObjectId(nameOfTileItem + tileRef.name);
            m_rangeTileManager.MoveTile(id, offset);
        }
        public void ClearRangeTile(string nameOfTileItem, GameObject tileRef) 
        {
            ObjectId id = new ObjectId(nameOfTileItem + tileRef.name);
            m_rangeTileManager.RemoveTiles(id);
        }
        #endregion

        #region Utility for Show/ToggleRangeMap
        void Show_Internal(
            ObjectId id,
            RangeMap rangeMap,
            GameObject tileRef,
            Transform parent)
        {
            m_rangeTileManager.Hide(tileRef);
            Show(id, rangeMap, tileRef, parent);
        }

        // true - Display is now active
        // false - Display is now not active
        bool Toggle_Internal(
            ObjectId id,
            RangeMap rangeMap,
            GameObject tileRef,
            Transform parent)
        {
            bool tilesActive = m_rangeTileManager.AreTilesActive(id);
            if (tilesActive)
            {
                HidePrompt();
                return !tilesActive;
            }
            m_rangeTileManager.Hide(tileRef);

            Show(id, rangeMap, tileRef, parent);
            return true;
        }
        #endregion
    }
}
