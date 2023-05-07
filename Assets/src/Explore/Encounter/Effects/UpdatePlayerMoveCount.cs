using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{    
    [Serializable]
    //[CreateAssetMenu(fileName = "UpdatePlayerMoveCount", menuName = "Curry/Encounter/Effects/UpdatePlayerMoveCount", order = 3)]
    public class UpdatePlayerMoveCount : BaseEncounterEffect, IEncounterModule
    {      
        public override string[] SerializePropertyNames => new string[]{};
        public override IEnumerator Activate(GameStateContext context) 
        { 
            
            yield return null; 
        }
    }
}