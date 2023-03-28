using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public bool HasValidTarget<T_Target>(Vector3 userOrigin, out List<T_Target> validTargets)
        {
            validTargets = new List<T_Target>();
            List<Vector3> validWorldPos = m_range.ApplyRangeOffsets(userOrigin);
            foreach (Vector3 pos in validWorldPos)
            {
                if (GameUtil.TrySearchTarget(pos, m_targetLayer, out T_Target found))
                {
                    validTargets.Add(found);
                }
            }
            return validTargets.Count > 0;
        }
        public void SetTarget(Vector3 target)
        {
            Target = target;
            Satisfied = true;
        }
        // User activating an effect with the targets selected
        public virtual IEnumerator ActivateWithTargets<T_Target, T_User>
            (T_User user, Action<T_Target, T_User> effect)
        {
            if (GameUtil.TrySearchTarget(Target, m_targetLayer, out T_Target found))
            {
                effect(found, user);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
