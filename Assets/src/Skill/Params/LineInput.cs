using System.Collections.Generic;
using UnityEngine;

namespace Curry.Skill
{
    public class LineInput : SkillInput 
    { 
        public List<Vector2> Vertices { get; private set; }
        public LineInput(List<Vector2> verts, Dictionary<string, object> payload = null): base(payload)
        {
            Vertices = verts;
        }
    }
}
