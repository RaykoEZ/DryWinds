using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "SpawnNearby", menuName = "Curry/Encounter/Effects/SpawnNearby", order = 3)]
    public class SpawnNearby : BaseEncounterEffect, IEncounterModule
    {
        public override string[] SerializePropertyNames => new string[] { };
        public override IEnumerator Activate(GameStateContext context)
        {
            yield return null;
        }
    }
}