using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Flee", order = 0)]
    public class AiFlee : AiAction<IActionInput>
    {
        public override float Priority(AiWorldState args)
        {
            float mod = args.Emotion.Fear - (0.5f * args.Emotion.Hatred);
            return mod * m_basePriority;
        }
        public override ICharacterAction<IActionInput> Execute(NpcController controller, AiWorldState state)
        {
            return null;
        }
    }
}
