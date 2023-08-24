using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_Healing", menuName = "Curry/Card/Resource/Healing Asset", order = 1)]
    public class Healing_Asset : CardAsset
    {
        [SerializeField] Healing m_resource = default;
        public override CardResource GetResource()
        {
            return new Healing(m_resource);
        }
    }
}
