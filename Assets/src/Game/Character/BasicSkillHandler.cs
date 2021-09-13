using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public class BasicSkillHandler : SkillHandler
    {        
        public bool IsSkillAvailable
        {
            get
            {
                return CurrentSkill.SkillUsable;
            }
        }

        public void InitSkill(Asset skillAsset, BaseCharacter user, bool hitBoxOn = false)
        {
            PrepareSkill(skillAsset);
            CurrentSkill.Init(user, hitBoxOn);
        }

        public virtual void SkillWindup()
        {           
            CurrentSkill.SkillWindup();
        }

        public override void ActivateSkill(Vector2 targetPos, Dictionary<string, object> payload = null)
        {
            SkillParam param = new SkillParam(targetPos, payload);
            ExecuteSkill(param);
        }

        public void InterruptSkill()
        {
            if (CurrentSkill.IsWindingUp)
            {
                CurrentSkill.CancelWindup();
            }
            else
            {
                CurrentSkill.EndSkillEffect();
            }
        }
    }
}
