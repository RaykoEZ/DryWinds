using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "MovementPointChange_", menuName = "Curry/Effects/MovementPointChange", order = 1)]
    public class MovementPointChange_EffectResource : BaseEffectResource 
    {
        [SerializeField] MovementPointChange m_movePointChange = default;
        public MovementPointChange MovePointChange => m_movePointChange;
        public override void Activate(GameStateContext context)
        {
            m_movePointChange?.ApplyEffect(context.Player);
        }
    }
}
