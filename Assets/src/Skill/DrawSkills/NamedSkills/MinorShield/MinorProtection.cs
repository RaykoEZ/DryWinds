using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public class MinorProtection : EdgeSkill
    {
        [SerializeField] float m_duration = default;

        protected override IEnumerator SkillEffect(IActionInput target)
        {
            Debug.Log("Draw Skill Activate");
            // Start animation
            yield return null;
        }
    }
}
