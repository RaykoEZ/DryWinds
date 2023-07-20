using System;
using UnityEngine;
using TMPro;
using Curry.UI;

namespace Curry.Explore
{
    //When player: moves, plays card, reformulate hand, this class handles cost previews 
    public class ActionCostHandler : MonoBehaviour 
    {
        [SerializeField] TimeManager m_time = default;
        [SerializeField] ActionCounter m_actionCounter = default;
        [SerializeField] GameMessageTrigger m_message = default;
        public static readonly ActionCost BaseMovementCost = 
            new ActionCost { ActionPoint = 1, Time = 1 };

        public bool HasEnoughResource(ActionCost cost, bool errorMessage = false) 
        {
            bool enoughTime = cost.Time <= m_time.TimeLeftToClear;
            bool enoughAp = cost.ActionPoint <= m_actionCounter.CurrentActionPoint;
            bool ret = enoughTime && enoughAp;
            if (errorMessage && !enoughTime) 
            {
                m_message?.TriggerGameMessage(ErrorMessages.s_notEnoughTime);
            }
            if (errorMessage && !enoughAp) 
            {
                m_message?.TriggerGameMessage(ErrorMessages.s_notEnoughAp);
            }
            return ret;
        }
        public bool TrySpend(ActionCost cost) 
        {
            bool ret = HasEnoughResource(cost, true);
            if (ret) 
            {
                m_time.TrySpendTime(cost.Time);
                m_actionCounter.UpdateMoveCountDisplay(m_actionCounter.CurrentActionPoint - cost.ActionPoint);
            }
            return ret;
        }
        public void BeginPreview(ActionCost cost) 
        {
            m_time?.PreviewCost(cost.Time);
            m_actionCounter?.PreviewCost(cost.ActionPoint);
        }
        public void CancelPreview() 
        {
            m_time?.CancelPreview();
            m_actionCounter?.CancelPreview();
        }
    }
}