using System;
using Curry.Game;

namespace Curry.Skill
{
    [Serializable]
    public struct SkillProperty 
    {
        public string Name;
        public ObjectRelations TargetOptions;
        public float SpCost;
        public float CooldownTime;
        public float MaxWindupTime;
        public float SkillValue;
        public float Knockback;
    }
}
