using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Skill;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Skill", order = 0)]
    public class AiSkill : AiState
    {
        public override bool PreCondition(NpcWorldState args) 
        {
            return args.Enemies.Count > 0;
        }

        public override void OnEnter(NpcController controller, NpcWorldState state)
        {

        }

        public override void OnUpdate(NpcController controller, NpcWorldState state)
        {

        }

    }

}
