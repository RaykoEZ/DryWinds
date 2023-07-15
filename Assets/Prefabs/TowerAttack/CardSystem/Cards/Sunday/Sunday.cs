using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{   
    public class Sunday : AdventCard, IConsumable
    {
        [SerializeField] GainAp_EffectResource m_actionGain = default;
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            m_actionGain?.Activate(context);
            yield return null;
        }
        public IEnumerator OnExpend()
        {
            yield return null;
        }
    }
}