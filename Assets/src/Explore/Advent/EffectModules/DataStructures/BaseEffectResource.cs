using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public abstract class BaseEffectResource : ScriptableObject 
    {
        // Activate without user and target in argument, will have player as the only target/user available
        public virtual void Activate(GameStateContext context) { }
    }
}
