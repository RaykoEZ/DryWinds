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

        public void Activate()
        {
            Debug.Log("AWAKE now");
            IsActive = true;
        }
        public void Hibernate()
        {
            Debug.Log("ASLEEP now");
            IsActive = false;
        }
    }
}
