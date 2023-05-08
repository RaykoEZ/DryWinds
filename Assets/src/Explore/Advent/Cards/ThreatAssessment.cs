using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public class ThreatAssessment : AdventCard, ICooldown
    {
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            Scan scan = (m_resources as Scan_CardResource).ScanModule;
            scan?.ApplyEffect(user, user);
            yield return new WaitForEndOfFrame();
        }
    }
}
