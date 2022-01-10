using System;
using UnityEngine;
using Curry.Game;
using System.Collections.Generic;
using Curry.Skill;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Chase", order = 0)]
    public class AiChase : AiAction<IActionInput>
    {
        public override float Priority(AiWorldState args)
        {
            float mod = args.Emotion.Emnity - args.Emotion.Fear;
            return mod * m_basePriority;
        }

        public override bool PreCondition(AiWorldState args)
        {
            return args.Enemies.Count > 0;
        }

        public override ICharacterAction<IActionInput> Execute(NpcController controller, AiWorldState state)
        {
            Transform target = ChooseTarget(state.Enemies).transform;
            controller.Move(target);
            return null;
        }
    }
}
