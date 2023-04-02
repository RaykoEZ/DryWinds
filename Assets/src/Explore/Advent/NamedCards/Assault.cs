using Curry.Util;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    // A basic card that requires targeting a tile in close range to activate
    public class Assault : AdventCard, ITargetsPosition, ICooldown
    {
        [SerializeField] DealDamageTo m_dealDamage = default;
        [SerializeField] CooldownModule m_cooldown = default;
        [SerializeField] PositionTargetingModule m_targeting = default;
        public RangeMap Range => m_targeting.Range;
        public bool Satisfied => m_targeting.Satisfied;
        public Vector3 Target => m_targeting.Target;

        public bool IsOnCooldown => ((ICooldown)m_cooldown).IsOnCooldown;

        public int CooldownTime { get => ((ICooldown)m_cooldown).CooldownTime; set => ((ICooldown)m_cooldown).CooldownTime = value; }
        public int Current { get => ((ICooldown)m_cooldown).Current; set => ((ICooldown)m_cooldown).Current = value; }

        public void SetTarget(Vector3 target)
        {
            m_targeting.SetTarget(target);
        }
        public override IEnumerator ActivateEffect(IPlayer user)
        {
            yield return new WaitForEndOfFrame();
            yield return m_targeting.
                ActivateWithTargets<ICharacter, IPlayer>(user, m_dealDamage.ApplyEffect);
        }

        public void TrggerCooldown()
        {
            ((ICooldown)m_cooldown).TrggerCooldown();
        }

        public void Tick(int dt, out bool isOnCoolDown)
        {
            ((ICooldown)m_cooldown).Tick(dt, out isOnCoolDown);
        }
    }
}
