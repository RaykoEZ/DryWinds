using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public abstract class BaseEffectResource : ScriptableObject 
    {
        public abstract void Activate(GameStateContext context);
    }

}
