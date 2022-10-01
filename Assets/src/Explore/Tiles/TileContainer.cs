using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    // Stores tile gameobjects generated in a session.
    public class TileContainer
    {
        List<TileItem> m_tiles = new List<TileItem>();
        public bool IsActive { get; protected set; } = false;
        public bool IsEmpty { get { return m_tiles.Count == 0; } }
        public TileContainer()
        {
        }

        public virtual void AddTileItem(TileItem item)
        {
            m_tiles.Add(item);
            IsActive = item.TileObject.activeSelf;
        }

        public virtual void HideTiles()
        {
            foreach (TileItem o in m_tiles)
            {
                o.TileObject?.SetActive(false);
            }
            IsActive = false;
        }

        public virtual void ShowTiles()
        {
            foreach (TileItem o in m_tiles)
            {
                o.TileObject?.SetActive(true);
            }
            IsActive = true;
        }

        public virtual void MoveTiles(Vector3 diff)
        {
            foreach (TileItem o in m_tiles)
            {
                o.TileObject.transform.position += diff;
            }
        }

        public virtual void MoveTilesTo(Vector3 pos)
        {
            foreach (TileItem o in m_tiles)
            {
                o.TileObject.transform.position = pos;
            }
        }

        public virtual void DestroyTiles()
        {
            IsActive = false;
            foreach (TileItem o in m_tiles)
            {
                Object.Destroy(o.TileObject);
            }
        }
    }
}