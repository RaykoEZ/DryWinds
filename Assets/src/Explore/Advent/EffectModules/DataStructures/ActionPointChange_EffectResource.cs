using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "ActionPointChange_", menuName = "Curry/Effects/ActionPointChange", order = 1)]
    public class ActionPointChange_EffectResource : BaseEffectResource 
    {
        [SerializeField] MovementPointChange m_movePointChange = default;
        public MovementPointChange MovePointChange => m_movePointChange;
        public override void Activate(GameStateContext context)
        {
            m_movePointChange?.ApplyEffect(context.ActionCount);
        }
    }
}
