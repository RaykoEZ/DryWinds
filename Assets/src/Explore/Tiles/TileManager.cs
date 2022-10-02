using UnityEngine;

namespace Curry.Explore
{
    public struct TileItem
    {
        public GameObject TileObject;
    }

    public class TileManager : MonoBehaviour
    {
        protected TileSpawner m_spawner = new TileSpawner();
        TileContainer m_displayTiles = new TileContainer();
        public bool IsActive { get { return !m_displayTiles.IsEmpty && m_displayTiles.IsActive; } }
        public bool IsEmpty { get { return m_displayTiles.IsEmpty; } }
        public void Add(GameObject objectRef, Vector3 pos, Transform parent)
        {
            GameObject o = m_spawner.SpawnTile(objectRef, pos, parent, false);
            TileItem item = new TileItem { TileObject = o };
            m_displayTiles.AddTileItem(item);
        }
        /// Tile Operations
        public virtual void Show()
        {
            m_displayTiles.ShowTiles();
        }

        public virtual void Hide()
        {
            m_displayTiles.HideTiles();
        }

        public virtual void MoveTile(Vector3 offset)
        {
            m_displayTiles.MoveTiles(offset);
        }
        public void MoveTileTo(Vector3 target) 
        {
            m_displayTiles.MoveTilesTo(target);
        }
        public void Clear() 
        {
            m_displayTiles.DestroyTiles();
        }
    }
}