using System.Collections.Generic;
using Curry.Game;

namespace Curry.Skill
{
    public class CharacterParam : SkillInput
    {
        public BaseCharacter Target { get; private set; }

        public CharacterParam(ITargetable<BaseCharacter> target, Dictionary<string, object> payload = null)
             : base(payload)
        {
            Target = target.Value;
        }
    }
}
