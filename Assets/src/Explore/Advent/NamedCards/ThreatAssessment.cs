using UnityEngine;

namespace Curry.Explore
{
    public class ThreatAssessment : AdventCard
    {
        [SerializeField] Scan m_scan = default;
        protected override void ActivateEffect(IPlayer user)
        {
            m_scan.ApplyEffect(user, user);
            OnExpend();
        }
    }
}
