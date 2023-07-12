using UnityEngine;

namespace Curry.Explore
{
    // Checks game state to criteras set
    public abstract class GameStateCondition : ScriptableObject
    {
        public abstract bool IsConditionMet(GameStateContext context);
    }
}