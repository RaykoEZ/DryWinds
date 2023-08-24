using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "MoveTo_", menuName = "Curry/Effects/MoveTo", order = 1)]
    public class MoveTo_EffectResource : BaseEffectResource
    {        
        [SerializeField] MoveTo m_effect = default;
        public MoveTo Effect => m_effect;
    }
}