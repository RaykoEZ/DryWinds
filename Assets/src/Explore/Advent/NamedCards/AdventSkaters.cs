using UnityEngine;
using Curry.Events;
using Curry.Util;

namespace Curry.Explore
{
    public class AdventSkaters : AdventCard
    {
        [SerializeField] CurryGameEventTrigger m_onMove = default;
        protected override void ActivateEffect(AdventurerStats user)
        {
            Vector3 dest = user.WorldPosition + VectorExtension.RandomCardinalVector();
            PositionInfo info = new PositionInfo(dest);
            m_onMove?.TriggerEvent(info);
            base.ActivateEffect(user);
            OnExpend();
        }
    }
}
