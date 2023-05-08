using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using Curry.Util;
using System;

namespace Curry.Explore
{
    [Serializable]
    public class Reveal
    {
        [SerializeField] int m_scoutRange = default;
        [SerializeField] CurryGameEventTrigger m_defog = default;
       
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            List<Vector3> worldPositions = RangeMapping.GetRangePositions(m_scoutRange, target.WorldPosition);
            RangeInfo info = new RangeInfo(worldPositions);
            m_defog?.TriggerEvent(info);
        }
    }
}
