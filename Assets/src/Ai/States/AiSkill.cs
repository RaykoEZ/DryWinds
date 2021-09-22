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
        List<BaseSkill> m_skillRefs;

        public override bool PreCondition(NpcWorldState args) 
        {
            return args.Enemies.Count > 0;
        }

        public override bool ExitCondition(NpcWorldState args) 
        {
            return args.Enemies.Count == 0 && !ActionInProgress;
        }

        public override void OnEnter(NpcController controller, NpcWorldState state)
        {
            m_skillRefs = state.Skills;

            throw new NotImplementedException();
        }

        public override void OnUpdate(NpcController controller, NpcWorldState state)
        {
            
            throw new NotImplementedException();
        }

        public override void TransitionTo(AiState next)
        {
            m_skillRefs.Clear();
            base.TransitionTo(next);
        }
    }

}
