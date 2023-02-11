using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public abstract class EncounterEffect : ScriptableObject, IEncounterModule
    {
        public abstract IEnumerator Activate(GameStateContext context); 
    }
}