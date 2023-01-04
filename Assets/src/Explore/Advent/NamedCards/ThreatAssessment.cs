using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    public class ThreatAssessment : AdventCard
    {
        [Range(0, 3)]
        [SerializeField] int m_detectionLevel = default;
        [SerializeField] CurryGameEventTrigger m_onSonar = default;

        protected override void ActivateEffect(IPlayer user)
        {
            ScanInfo info = new ScanInfo(m_detectionLevel, 3f);
            m_onSonar?.TriggerEvent(info);
            OnExpend();
        }
    }
}
