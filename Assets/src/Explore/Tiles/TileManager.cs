using System.Collections.Generic;
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
        Dictionary<ObjectId, TileContainer> m_containers = new Dictionary<ObjectId, TileContainer>();
        public bool DoTilesExist(ObjectId id)
        {
            if (id == null)
            {
                return false;
            }
            return m_containers.TryGetValue(id, out TileContainer container) && container != null;
        }

        public bool AreTilesActive(ObjectId id)
        {
            if (id == null)
            {
                return false;
            }
            return DoTilesExist(id) && m_containers[id].IsActive;
        }
        public void Add(ObjectId id, GameObject objectRef, Vector3 pos, Transform parent)
        {
            GameObject o = m_spawner.SpawnTile(objectRef, pos, parent, false);
            TileItem item = new TileItem { TileObject = o };
            if (!m_containers.TryGetValue(id, out TileContainer val) || val == null)
            {
                TileContainer newContainer = new TileContainer();
                newContainer.AddTileItem(item);
                m_containers.Add(id, newContainer);
            }
            else 
            {
                val.AddTileItem(item);
            }

        }
        /// Tile Operations
        public virtual void Show(ObjectId id)
        {
            if (m_containers.TryGetValue(id, out TileContainer val) && val != null)
            {
                val.ShowTiles();
            }
        }

        public virtual void Hide(GameObject objectRef)
        {
            ObjectId id = new ObjectId(objectRef);
            if (m_containers.TryGetValue(id, out TileContainer val) && val != null)
            {
                val.HideTiles();
            }
        }

        public virtual void HideAll()
        {
            foreach (TileContainer c in m_containers.Values)
            {
                c.HideTiles();
            }
        }

        public virtual void MoveTile(GameObject objectRef, Vector3 offset)
        {
            ObjectId id = new ObjectId(objectRef);
            if (m_containers.TryGetValue(id, out TileContainer val) && val != null)
            {
                val.MoveTiles(offset);
            }
        }
        public void MoveTileTo(GameObject objectRef, Vector3 target) 
        {
            ObjectId id = new ObjectId(objectRef);
            if (m_containers.TryGetValue(id, out TileContainer val) && val != null)
            {
                val.MoveTilesTo(target);
            }
        }

        public virtual void RemoveTiles(ObjectId id)
        {
            if (id == null)
            {
                return;
            }
            m_containers[id]?.DestroyTiles();
            m_containers.Remove(id);
        }
    }
}