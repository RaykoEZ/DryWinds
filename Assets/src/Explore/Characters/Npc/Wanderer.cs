using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    // A basic enemy with stealth level
    public class Wanderer : TacticalEnemy, IStealthy
    {
        [Range(1, 3)]
        [SerializeField] int m_stealthLevel = default;
        [SerializeField] Push m_push = default;
        public int StealthLevel => m_stealthLevel;

        protected override void Defeat()
        {
            m_anim.SetBool("isInActive", false);
            base.Defeat();
        }
        protected override IEnumerator ExecuteAction_Internal()
        {
            yield return base.ExecuteAction_Internal();
            Reveal();
            if (m_targetsInSight.Count == 0) 
            {
                Hide();
            }
            foreach(IPlayer player in m_targetsInSight) 
            {
                player.TakeHit(1);
                m_push.ApplyEffect(player, this);
            }
            yield return null;
        }
    }
}
