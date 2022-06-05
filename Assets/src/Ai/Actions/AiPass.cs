using Curry.Game;
using System;
using UnityEngine;

namespace Curry.Ai
{
    [Serializable]
    [AddComponentMenu("Curry/Ai Action/Pass")]
    public class AiPass : AiAction<IActionInput>
    {
        public override bool PreCondition(AiWorldState args)
        {
            return true;
        }

        protected override void ExecuteAction(AiActionInput param)
        {
        }
    }
}
