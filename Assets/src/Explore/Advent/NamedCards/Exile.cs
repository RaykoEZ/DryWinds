using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public class Exile : AdventCard, ITargetsPosition
    {
        [SerializeField] Push m_push = default;
        [SerializeField] PositionTargetingModule m_targeting = default;
        public int Range => ((ITargetsPosition)m_targeting).Range;

        public bool Satisfied => ((IActivationCondition<Vector3>)m_targeting).Satisfied;

        public Vector3 Target => ((IActivationCondition<Vector3>)m_targeting).Target;

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
    }
}
