using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public interface ITimeLimit
    { 
        float Duration { get; }
        float TimeElapsed { get; }
        void OnExpire();
    }

    public class FragileBarrier : FragileObject
    { 
    
    }

    public class MinorShield : EdgeSkill
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
