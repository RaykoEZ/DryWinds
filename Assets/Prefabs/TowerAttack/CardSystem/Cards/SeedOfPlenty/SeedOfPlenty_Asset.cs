using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_SeedOfPlenty", menuName = "Curry/Card/Resource/SeedOfPlenty Asset", order = 1)]
    public class SeedOfPlenty_Asset : CardAsset
    {
        [SerializeField] SeedOfPlenty m_resource = default;
        public override CardResource GetResource()
        {
            return new SeedOfPlenty(m_resource);
        }
    }
}