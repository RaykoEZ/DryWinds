using System.Collections.Generic;
using Curry.Game;

namespace Curry.Skill
{
    public class SkillParam : IActionParam
    {
        public Dictionary<string, object> Payload { get; private set; }
        public SkillParam(Dictionary<string, object> payload = null)
        {
            Payload = payload;
        }
    }
}
