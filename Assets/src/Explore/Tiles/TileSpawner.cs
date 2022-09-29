using System.Collections.Generic;
using UnityEngine;
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
}