using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using Curry.Events;
namespace Curry.Explore
{
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
    public delegate void OnTileSelect(Vector3Int tileCoord);
    public class TileSelectionManager : MonoBehaviour
    {
        [SerializeField] GameObject m_highlightObject = default;
        [SerializeField] Tilemap m_map = default;
        [SerializeField] TileManager m_tileHighlightManager = default;

        public event OnTileSelect OnTileSelected = default;

        Vector3Int m_lastMouseCoordinate = default;

        void Awake()
        {
            m_tileHighlightManager.Clear();
            m_tileHighlightManager.Add(m_highlightObject, Vector3.zero, transform);
        }

        void HighlightTileInternal(Vector3Int gridCoord)
        {
            bool coordMoved = m_lastMouseCoordinate != gridCoord;
            if (coordMoved)
            {
                Vector3Int diff = gridCoord - m_lastMouseCoordinate;

                m_tileHighlightManager.MoveTile(diff);
                m_lastMouseCoordinate = gridCoord;
            }
            m_tileHighlightManager.Show();
        }

        public void HandleHighlightTile()
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3Int gridCoord = m_map.WorldToCell(worldPos);
            OnTileSelected?.Invoke(gridCoord);
            HighlightTileInternal(gridCoord);
        }



    }
}