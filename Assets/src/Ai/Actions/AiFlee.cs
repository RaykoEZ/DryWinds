using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Flee", order = 0)]
    public class AiFlee : AiAction<IActionInput>
    {
        public override bool PreCondition(AiWorldState args)
        {
            return args.Enemies.Count > 0 && args.Emotion.EmotionState == AiEmotionState.Fear;
        }

        public override float Priority(AiWorldState args)
        {
            float mod = args.Emotion.Fear - (0.5f * args.Emotion.Hatred);
            return mod * m_basePriority;
        }

        public override ICharacterAction<IActionInput> Execute(NpcController controller, AiWorldState state)
        {
            Debug.Log("Fleeing!!!");
            return null;
        }
    }
}
