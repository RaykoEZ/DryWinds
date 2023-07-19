using System;
using UnityEngine;
using Curry.Events;
using UnityEngine.Tilemaps;

namespace Curry.Explore
{
    [Serializable]
    public class TacticalSpawnProperties 
    {
        [SerializeField] public LayerMask DoNotSpawnOn;
        [SerializeField] public Tilemap SpawnMap;
        [SerializeField] public CurryGameEventListener OnSpawn;
        [SerializeField] public BaseBehaviourInstanceManager InstanceManager;
    }
}