using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public abstract class BaseEncounterEffect : ScriptableObject, IEncounterModule
    {
        public abstract string[] SerializePropertyNames { get; }
        public abstract IEnumerator Activate(GameStateContext context);
    }
}