using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    // A basic card that requires targeting a tile in close range to activate
    public class Assault : AdventCard, ITargetsPosition
    {
        [SerializeField] DealDamageTo m_dealDamage = default;
        [SerializeField] PositionTargetingModule m_targeting = default;
        public int Range => m_targeting.Range;
        public bool Satisfied => m_targeting.Satisfied;
        public Vector3 Target => m_targeting.Target;
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
    }
}
