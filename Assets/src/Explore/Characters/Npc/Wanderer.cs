using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    // A basic enemy with stealth level
    public class Wanderer : TacticalEnemy, IStealthy, IOrganicLife
    {
        [Range(1, 3)]
        [SerializeField] int m_stealthLevel = default;
        [SerializeField] Push m_push = default;
        [SerializeField] ActiveTimeFrame m_activeTime = default;
        public int StealthLevel => m_stealthLevel;
        public ActiveTimeFrame ActiveHours => m_activeTime;

        public bool IsActive { get; protected set; }
        protected override void Defeat()
        {
            m_anim.SetBool("isInActive", false);
            base.Defeat();
        }
        protected override void OnDetect() 
        {
            if (IsActive) 
            {
                base.OnDetect();
            }
        }

        public override bool UpdateCountdown(int dt)
        {
            if (!IsActive) return false;
            return base.UpdateCountdown(dt);
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
        public void Activate()
        {
            IsActive = true;
            m_anim.SetBool("isInActive", false);
        }
        public void Hibernate()
        {
            if (!m_detect.PlayerDetected) 
            {
                IsActive = false;
                m_anim.SetBool("isInActive", true);
            }
        }
    }
}
