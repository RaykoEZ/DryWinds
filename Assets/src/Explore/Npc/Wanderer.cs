using UnityEngine;
namespace Curry.Explore
{
    // A basic enemy with stealth level
    public class Wanderer : TacticalEnemy, IStealthy
    {
        [Range(1, 3)]
        [SerializeField] int m_stealthLevel = default;
        public int StealthLevel => m_stealthLevel;
    }
}
