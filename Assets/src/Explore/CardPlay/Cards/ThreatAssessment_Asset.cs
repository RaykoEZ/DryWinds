using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_ThreatAssessment", menuName = "Curry/Card/Resource/ThreatAssessment Asset", order = 1)]
    public class ThreatAssessment_Asset : CardAsset
    {
        [SerializeField] ThreatAssessment m_resource = default;
        public override CardResource GetResource()
        {
            return new ThreatAssessment(m_resource);
        }
    }
}
