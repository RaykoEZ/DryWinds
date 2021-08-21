using System;
using Curry.Game;

namespace Curry.Skill
{
    [Serializable]
    public struct SkillProperty 
    {
        public ObjectRelations TargetOptions;
        public float SpCost;
        public float CooldownTime;
        public float MaxWindupTime;
        public float StaminaDamage;
        public float SpDamage;
        public float Knockback;
    }
}
