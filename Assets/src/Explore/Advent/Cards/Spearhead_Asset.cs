using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_Spearhead", menuName = "Curry/Card/Resource/Spearhead Asset", order = 1)]
    public class Spearhead_Asset : CardAsset
    {
        [SerializeField] Spearhead m_resource = default;
        public override CardResource GetResource()
        {
            return new Spearhead(m_resource);
        }
    }
}