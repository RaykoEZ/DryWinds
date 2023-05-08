using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class MovementPointerChange_EffectResource : BaseEffectResource 
    {
        [SerializeField] MovementPointChange m_movePointChange = default;
        public MovementPointChange MovePointChange => m_movePointChange;
    }
}
