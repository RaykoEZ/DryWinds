using System.Collections.Generic;
using Curry.Game;

namespace Curry.Skill
{
    public class SkillInput : IActionInput
    {
        public Dictionary<string, object> Payload { get; private set; }
        public SkillInput(Dictionary<string, object> payload = null)
        {
            Payload = payload;
        }
    }
}
