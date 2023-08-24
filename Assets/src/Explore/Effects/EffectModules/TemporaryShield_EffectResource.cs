using UnityEngine;
namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "TempShield_", menuName = "Curry/Effects/TempShield", order = 1)]
    public class TemporaryShield_EffectResource : DamageReduction_EffectResource<TemporaryShield>
    {
        [SerializeField] TemporaryShield m_shieldEffect = default;
        public override TemporaryShield Effect => m_shieldEffect;
    }
}