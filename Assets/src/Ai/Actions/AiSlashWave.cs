using Curry.Game;
using Curry.Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Ai
{
    [Serializable]
    [AddComponentMenu("Curry/Ai Action/SlashWave")]
    public class AiSlashWave : AiAction<IActionInput>
    {
        public override bool PreCondition(AiWorldState args)
        {
            return base.PreCondition(args) && args.Enemies.Count > 0;
        }

        protected override void ExecuteAction(AiActionInput param)
        {
            param.Controller.EquipeSkill("SlashWave");
            param.Controller.OnBasicSkill(param.WorldState.Enemies[0]);
        }
    }
}
