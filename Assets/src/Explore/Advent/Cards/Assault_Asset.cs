using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Card_Assault", menuName = "Curry/Card/Resource/Assault Asset", order = 1)]
    public class Assault_Asset : CardAsset
    {
        [SerializeField] Assault m_assault = default;
        public override CardResource GetResource()
        {
            return new Assault(m_assault);
        }
    }
}
