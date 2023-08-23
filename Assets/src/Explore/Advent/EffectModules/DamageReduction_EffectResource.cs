using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    public abstract class DamageReduction_EffectResource<T> : BaseEffectResource where T : DamageReduction
    {      
        public abstract T Effect { get; }
    }
}