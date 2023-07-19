using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_Disengage", menuName = "Curry/Card/Resource/Disengage Asset", order = 1)]
    public class Disengage_Asset : CardAsset
    {
        [SerializeField] Disengage m_resource = default;
        public override CardResource GetResource()
        {
            return new Disengage(m_resource);
        }
    }
}