using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using Curry.Util;

namespace Curry.Explore
{
    public class SweepScanner : AdventCard
    {
        [SerializeField] int m_scoutRange = default;
        [SerializeField] CurryGameEventTrigger m_scan = default;
        RangeMap m_rangeMap;
        protected override void Awake()
        {
            base.Awake();
            m_rangeMap = RangeMapping.GetNeighbourRangeMap(m_scoutRange);
        }

        protected override void ActivateEffect(IPlayer user)
        {
            List<Vector3> worldPositions = m_rangeMap.ApplyRangeOffsets(user.CurrentStats.WorldPosition);
            RangeInfo info = new RangeInfo(worldPositions);
            m_scan?.TriggerEvent(info);
            OnExpend();
        }
    }
}
