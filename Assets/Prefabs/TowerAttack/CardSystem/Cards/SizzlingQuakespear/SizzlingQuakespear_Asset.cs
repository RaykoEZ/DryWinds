using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_SizzlingQuakespear", menuName = "Curry/Card/Resource/SizzlingQuakespear Asset", order = 1)]
    public class SizzlingQuakespear_Asset : CardAsset
    {
        [SerializeField] SizzlingQuakespear m_resource = default;
        public override CardResource GetResource()
        {
            return new SizzlingQuakespear(m_resource);
        }
    }
}