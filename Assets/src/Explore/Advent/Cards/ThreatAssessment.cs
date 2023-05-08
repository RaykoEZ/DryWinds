using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public class ThreatAssessment : AdventCard, ICooldown
    {
        [SerializeField] Scan_EffectResource m_scan = default;
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            m_scan?.ScanModule?.ApplyEffect(user, user);
            yield return new WaitForEndOfFrame();
        }
    }
}
