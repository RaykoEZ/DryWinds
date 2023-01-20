using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public class ThreatAssessment : AdventCard
    {
        [SerializeField] Scan m_scan = default;
        public override IEnumerator ActivateEffect(IPlayer user)
        {
            m_scan.ApplyEffect(user, user);
            yield return null;
            OnExpend();
        }
    }
}
