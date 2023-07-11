using System;
using UnityEngine;
using TMPro;

namespace Curry.Explore
{
    //When player: moves, plays card, reformulate hand, this class handles cost previews 
    public class ActionCostHandler : MonoBehaviour 
    {
        [SerializeField] TimeManager m_time = default;
        [SerializeField] ActionCounter m_actionCounter = default;
        public static readonly ActionCost BaseMovementCost = 
            new ActionCost { ActionCount = 1, Time = 1 };
        public bool HasEnoughResource(ActionCost cost) 
        {
            return cost.Time <= m_time.TimeLeftToClear &&
                cost.ActionCount <= m_actionCounter.CurrentActionCount;
        }
        public bool TrySpend(ActionCost cost) 
        {
            bool ret = HasEnoughResource(cost);
            if (ret) 
            {
                m_time.TrySpendTime(cost.Time);
                m_actionCounter.UpdateMoveCountDisplay(m_actionCounter.CurrentActionCount - cost.ActionCount);
            }
            return ret;
        }
        public void BeginPreview(ActionCost cost) 
        {
            m_time?.PreviewCost(cost.Time);
            m_actionCounter?.PreviewCost(cost.ActionCount);
        }
        public void CancelPreview() 
        {
            m_time?.CancelPreview();
            m_actionCounter?.CancelPreview();
        }
    }
}