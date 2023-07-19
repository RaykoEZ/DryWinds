using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_Sunday", menuName = "Curry/Card/Resource/Sunday Asset", order = 1)]
    public class Sunday_Asset : CardAsset
    {
        [SerializeField] Sunday m_resource = default;
        public override CardResource GetResource() 
        {       
            return new Sunday(m_resource);
        }
    }
}