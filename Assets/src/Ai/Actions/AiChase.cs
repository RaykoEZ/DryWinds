using System;
using UnityEngine;
using Curry.Game;
using System.Collections.Generic;
using Curry.Skill;
using System.Collections;

namespace Curry.Ai
{
    [Serializable]
    [AddComponentMenu("Curry/Ai Action/Chase")]
    public class AiChase : AiAction<IActionInput>
    {
        public override bool PreCondition(AiWorldState args)
        {
            return args.Enemies.Count > 0;
        }

        public override void OnEnter(AiActionInput param)
        {
        }

        protected override void ExecuteAction(AiActionInput param)
        {
            throw new NotImplementedException();
        }
    }
}
