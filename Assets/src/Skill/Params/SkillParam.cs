using System.Collections.Generic;

namespace Curry.Skill
{
    public class SkillParam 
    {
        public Dictionary<string, object> Payload { get; private set; }
        public SkillParam(Dictionary<string, object> payload = null)
        {
            Payload = payload;
        }
    }
}
