using Curry.Util;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public class Exile : AdventCard, ITargetsPosition, ICooldown
    {
        [SerializeField] Push m_push = default;
        [SerializeField] CooldownModule m_cooldown = default;
        [SerializeField] PositionTargetingModule m_targeting = default;
        public RangeMap Range => ((ITargetsPosition)m_targeting).Range;

        public bool Satisfied => ((IActivationCondition<Vector3>)m_targeting).Satisfied;

        public Vector3 Target => ((IActivationCondition<Vector3>)m_targeting).Target;

        public bool IsOnCooldown => ((ICooldown)m_cooldown).IsOnCooldown;

        public int CooldownTime { get => ((ICooldown)m_cooldown).CooldownTime; set => ((ICooldown)m_cooldown).CooldownTime = value; }
        public int Current { get => ((ICooldown)m_cooldown).Current; set => ((ICooldown)m_cooldown).Current = value; }

        public override IEnumerator ActivateEffect(IPlayer user)
        {
            yield return new WaitForEndOfFrame();
            yield return m_targeting.
                ActivateWithTargets<ICharacter, IPlayer>(user, m_push.ApplyEffect);
        }
        public void SetTarget(Vector3 target)
        {
            ((IActivationCondition<Vector3>)m_targeting).SetTarget(target);
        }

        public void Tick(int dt, out bool isOnCoolDown)
        {
            ((ICooldown)m_cooldown).Tick(dt, out isOnCoolDown);
        }

        public void TrggerCooldown()
        {
            ((ICooldown)m_cooldown).TrggerCooldown();
        }
    }
}
