using System.Collections.Generic;
using UnityEngine;

namespace Curry.Skill
{

    // Skill to store target info and additional arguments for a skill activation
    public class VectorParam : SkillParam
    {
        public Vector2 Target { get; private set; }
        public VectorParam(ITargetable<Vector2> target, Dictionary<string, object> payload = null) 
            : base(payload) 
        {
            Target = target.Value;
        }
    }
}
