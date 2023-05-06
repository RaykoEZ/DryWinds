using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{  
    [Serializable]
    [CreateAssetMenu(fileName = "RevealSecret", menuName = "Curry/Encounter/Effects/RevealSecret", order = 3)]
    public class RevealSecret : BaseEncounterEffect, IEncounterModule
    {
        
        public override string[] SerializePropertyNames => new string[]{};
        public override IEnumerator Activate(GameStateContext context) 
        { 
            
            yield return null; 
        }
    }
}