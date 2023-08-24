using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_Shield", menuName = "Curry/Card/Resource/Shield Asset", order = 1)]
    
    public class Shield_Asset : CardAsset
    {       
        [SerializeField] Shield m_resource = default;      
        public override CardResource GetResource()
        {        
            return new Shield(m_resource);
        }
    }
}