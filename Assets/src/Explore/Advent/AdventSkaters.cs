using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    public class AdventSkaters : AdventCard
    {
        [SerializeField] CurryGameEventTrigger m_onMove = default;

        protected override void ActivateEffect(AdventurerStats user)
        {
            MovementInfo info = new MovementInfo().
                SetDirection(MovementInfo.MovementDirection.Up).
                SetMagnitude(1);
            m_onMove?.TriggerEvent(info);
            OnExpend();
        }
    }
}
