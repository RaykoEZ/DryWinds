using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public interface ISummon 
    { 
        float Duration { get; }
    }

    public class MinorShield : EdgeSkill, ISummon
    {
        [SerializeField] float m_duration = default;

        public float Duration { get { return m_duration; } }
        protected override void UpdateHitBox(List<Vector2> verts)
        {
            base.UpdateHitBox(verts);
        }

        protected override IEnumerator SkillEffect(IActionInput target)
        {
            yield return new WaitForSeconds(m_duration);
            Debug.Log("Shield Ends");
        }
    }
}
