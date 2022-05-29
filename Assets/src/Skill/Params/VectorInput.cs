using System.Collections.Generic;
using UnityEngine;

namespace Curry.Skill
{

    // Skill to store target info and additional arguments for a skill activation
    public class VectorInput : SkillInput
    {
        public Vector2 Value { get; private set; }
        public VectorInput(Vector2 value, Dictionary<string, object> payload = null) 
            : base(payload) 
        {
            Value = value;
        }
    }
}
