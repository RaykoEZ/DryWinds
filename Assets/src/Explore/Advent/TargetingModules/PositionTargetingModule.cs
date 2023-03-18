using UnityEngine;
using Curry.Util;
using System.Collections;
using System;
namespace Curry.Explore
{
    [Serializable]
    public class PositionTargetingModule : ITargetsPosition
    {
        [SerializeField] protected LayerMask m_targetLayer = default;
        [SerializeField] protected RangeMap m_range = default;
        public RangeMap Range => m_range;
        public bool Satisfied { get; protected set; }
        public Vector3 Target { get; protected set; }
        protected RaycastHit2D[] ValidTargets => GameUtil.SearchTargetPosition(Target, m_targetLayer);
        public void SetTarget(Vector3 target)
        {
            Target = target;
            Satisfied = true;
        }
        // User activating an effect with the targets selected
        public virtual IEnumerator ActivateWithTargets<T_Target, T_User>
            (T_User user, Action<T_Target, T_User> effect)
        {
            foreach (RaycastHit2D hit in ValidTargets)
            {
                if (hit.transform.TryGetComponent(out T_Target target))
                {
                    effect(target, user);
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
