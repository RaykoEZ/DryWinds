using System.Collections.Generic;
using Curry.Game;
using Curry.Skill;

namespace Curry.Ai
{
    public abstract class AiSkill : AiAction 
    {
        protected virtual BaseSkill CurrentAction { get; set; }

        public override void Execute(NpcController controller, NpcWorldState state)
        {
            ActivateSkill(controller, state);
        }

        protected virtual void ActivateSkill(NpcController controller, NpcWorldState state) 
        {
            BaseCharacter target = HeuristicUtil.WeakestCharacter(state.Enemies);
            TargetPosition pos = new TargetPosition(target.transform.position);
            CurrentAction = ChooseSkill(state.BasicSkills, pos);
            CurrentAction.OnFinish += OnActionFinished;
            controller.EquipBasicSkill(CurrentAction);
            controller.OnBasicSkill(pos);
        }

        protected abstract BaseSkill ChooseSkill(List<BaseSkill> skills, TargetPosition target);
        protected abstract float SkillScore(BaseSkill skill, TargetPosition target);

    }
}
