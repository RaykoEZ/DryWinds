using UnityEngine;
namespace Curry.Explore
{
    public class Exile : PositionTargetCard, ITargetsPosition
    {
        [SerializeField] Push m_push = default;
        protected override void Effect_Internal(ICharacter target, IPlayer user)
        {
            m_push.ApplyEffect(target, user);
            OnExpend();
        }
    }
}
