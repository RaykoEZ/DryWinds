using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_Onslaught", menuName = "Curry/Card/Resource/Onslaught Asset", order = 1)]
    public class Onslaught_Asset : CardAsset
    {
        [SerializeField] Onslaught m_resource = default;
        public override CardResource GetResource()
        {
            return new Onslaught(m_resource);
        }
    }
}