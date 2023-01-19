using UnityEngine;
namespace Curry.Explore
{
    // A basic card that requires targeting a tile in close range to activate
    public class Assault: PositionTargetCard, ITargetsPosition
    {
        [SerializeField] DealDamage m_dealDamage = default;

        protected override void Effect_Internal(ICharacter target, IPlayer user)
        {
            m_dealDamage.ApplyEffect(target, user);
            OnExpend();
        }
    }
}
