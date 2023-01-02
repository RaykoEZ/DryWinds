using UnityEngine;
namespace Curry.Explore
{
    // A basic enemy with stealth level
    public class Wanderer : TacticalEnemy, IStealthy, IOrganicLife
    {
        [Range(1, 3)]
        [SerializeField] int m_stealthLevel = default;
        [SerializeField] ActiveTimeFrame m_activeTime = default;
        public int StealthLevel => m_stealthLevel;

        public ActiveTimeFrame ActiveHours => m_activeTime;

        public bool IsActive { get; protected set; }
        protected override void Defeat()
        {
            m_anim.SetBool("isInActive", false);
            base.Defeat();
        }
        public override void OnDetect() 
        {
            if (IsActive) 
            {
                base.OnDetect();
            }
        }
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            foreach(IPlayer player in m_targetsInSight) 
            {
                player.TakeHit(1);
                
            }
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
