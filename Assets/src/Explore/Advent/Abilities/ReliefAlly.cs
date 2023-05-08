using Curry.Util;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class ReliefAlly : BaseAbility
    {
        [SerializeField] SwapPosition_EffectResource m_relief = default;
        public override AbilityContent Content
        {
            get
            {
                AbilityContent ret = base.Content;
                ret.Name = "Relief Ally";
                ret.Description = "Swap position with a nearby ally.";
                return ret;
            }
        }

        public void Activate(ICharacter target, ICharacter user)
        {
            m_relief?.SwapModule?.ApplyEffect(target, user);
        }
    }
}
