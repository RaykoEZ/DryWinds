using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_JustWater", menuName = "Curry/Card/Resource/JustWater Asset", order = 1)]
    public class JustWater_Asset : CardAsset
    {
        [SerializeField] JustWater m_resource = default;
        public override CardResource GetResource()
        {
            return new JustWater(m_resource);
        }
    }
}