using System.Collections.Generic;
using UnityEngine;

namespace Assets.src.Explore
{
    public struct TileItem
    {
        public GameObject TileObject;
    }

    // For spawning basic tiles on a map.
    public class TileSpawner
    {
        public virtual GameObject SpawnTile(
            GameObject objectRef, Vector3 position, Transform parent,
            bool isActive = true
            )
        {
            GameObject o = Object.Instantiate(objectRef, position, Quaternion.identity, parent);
            o.SetActive(isActive);
            return o;
        }
    }

    public class TileManager : MonoBehaviour
    {
        protected TileSpawner m_spawner = new TileSpawner();
        TileContainer m_displayTiles = new TileContainer();

        public virtual GameObject SpawnTile( GameObject objectRef, Vector3 position, Transform parent,
            bool isActive = true
            )
        {
            GameObject o = m_spawner.SpawnTile(objectRef, position, parent, isActive);
            TileItem item = new TileItem { TileObject = o};
            m_displayTiles.AddTileItem(item);
            return o;
        }

        /// Tile Operations

        public virtual void ShowTile()
        {
            m_displayTiles.ShowTiles();
        }

        public virtual void HideTile()
        {
            m_displayTiles.HideTiles();
        }

        public virtual void MoveTile(Vector3 offset)
        {
            m_displayTiles.MoveTiles(offset);
        }
    }



    // Stores tile gameobjects generated in a session.
    public class TileContainer
    {
        List<TileItem> m_tiles = new List<TileItem>();
        public bool isActive { get; protected set; } = false;

        public TileContainer()
        {
        }

        public virtual void AddTileItem(TileItem item)
        {
            m_tiles.Add(item);
            isActive = item.TileObject.activeSelf;
        }

        public virtual void HideTiles()
        {
            foreach (TileItem o in m_tiles)
            {
                o.TileObject?.SetActive(false);
            }
            isActive = false;
        }

        public virtual void ShowTiles()
        {
            foreach (TileItem o in m_tiles)
            {
                o.TileObject?.SetActive(true);
            }
            isActive = true;
        }

        public virtual void MoveTiles(Vector3 diff)
        {
            foreach (TileItem o in m_tiles)
            {
                o.TileObject.transform.position += diff;
            }
        }

        public virtual void DestroyTiles()
        {
            isActive = false;
            foreach (TileItem o in m_tiles)
            {
                Object.Destroy(o.TileObject);
            }
        }
    }
}