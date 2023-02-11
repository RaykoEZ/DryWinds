using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    // A template for creating a new item via visual studio
    [Serializable]
    [CreateAssetMenu(fileName = "New_", menuName = "Curry/Encounter/Effects/New", order = 1)]
    public class NewEncounterEffect : EncounterEffect, IEncounterModule
    {
        public override string[] SerializePropertyNames => new string[]{};
        public override IEnumerator Activate(GameStateContext context) 
        { 
            yield return null; 
        }
    }
}