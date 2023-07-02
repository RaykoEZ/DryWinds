using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{  
    public class Spearhead : AdventCard, ITargetsPosition, ICooldown
    {
        [SerializeField] DealDamage_EffectResource m_dealDamage = default;
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            throw new NotImplementedException();
        }
    }
}