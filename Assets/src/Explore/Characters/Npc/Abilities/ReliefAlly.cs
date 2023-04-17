using Curry.Util;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class ReliefAlly : BaseAbility, IAbility
    {
        [SerializeField] SwapPosition m_relief = default;

        protected override RangeMap Range => RangeMap.Adjacent;

        public override AbilityContent GetContent()
        {
            AbilityContent ret = base.GetContent();
            ret.Name = "Relief Ally";
            ret.Description = "Swap position with a nearby ally.";
            return ret;
        }

        public void Activate(ICharacter target, ICharacter user)
        {
            m_relief?.ApplyEffect(target, user);
        }
    }
}
