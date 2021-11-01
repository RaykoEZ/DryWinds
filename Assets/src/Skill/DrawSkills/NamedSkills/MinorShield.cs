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

    public class MinorShield : EdgeSkill, ITimeLimit
    {
        [SerializeField] float m_duration = default;

        public float Duration { get { return m_duration; } }
        public float TimeElapsed { get; protected set; }

        public override void OnHit(Interactable hit)
        {
            // Any trigger during effect time
        }

        public void OnExpire() 
        {
            m_animator.SetTrigger("End");
        }

        protected override IEnumerator SkillEffect(IActionInput target)
        {
            while(TimeElapsed < Duration) 
            {
                TimeElapsed += Time.deltaTime;
                yield return null;
            }

            OnExpire();
            Debug.Log("Shield Ends");
        }
    }
}
