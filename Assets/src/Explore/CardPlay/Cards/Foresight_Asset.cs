using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_Foresight", menuName = "Curry/Card/Resource/Foresight Asset", order = 1)]
    public class Foresight_Asset : CardAsset
    {
        [SerializeField] Foresight m_resource = default;
        public override CardResource GetResource()
        {
            return new Foresight(m_resource);
        }
    }
}
