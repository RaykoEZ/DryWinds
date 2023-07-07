using UnityEngine;

namespace Curry.Util
{
    [CreateAssetMenu(fileName = "RangeMapAsset", menuName = "Curry/Range/Range map cache asset", order = 1)]
    public class RangeMapAsset : ScriptableObject
    {
        [SerializeField] RangeMap m_range = default;
        public RangeMap Range { get => m_range; set => m_range = value; }
    }
}