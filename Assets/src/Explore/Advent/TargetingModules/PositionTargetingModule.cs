using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Curry.Explore
{
    [Serializable]
    public class PositionTargetingModule
    {
        [SerializeField] protected LayerMask m_targetLayer = default;
        public bool Satisfied { get; protected set; }
        public Vector3 Target { get; protected set; }
        public bool HasValidTargets<T_Target>(Vector3 userOrigin, RangeMap range, out List<T_Target> validTargets)
        {
            return GameUtil.HasValidTargets(userOrigin, range, m_targetLayer, out validTargets);
        }
        public bool TryGetCurrentTarget<T_Target>(out T_Target result) 
        {          
            return GameUtil.TrySearchTarget(Target, m_targetLayer, out result);
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
