using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{   
    public class Onslaught : AdventCard, ITargetsPosition
    {
        [SerializeField] DealDamage_EffectResource m_damage = default;
        [SerializeField] GainAp_EffectResource m_gainAp = default;
        [Range(0, 99)]
        [SerializeField] int m_damageIncrease = default;
        bool m_powerUp = false;
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            bool targetFound = m_targeting.TryGetCurrentTarget(out ICharacter found) && found != null;
            if (targetFound && m_powerUp) 
            {
                m_powerUp = false;
                m_damage.DamageModule.ApplyEffect(found, user, m_damageIncrease);
            }
            else if (targetFound && !m_powerUp) 
            {
                m_damage.DamageModule.ApplyEffect(found, user);
            }
            // Check if enemy is defeated after attack
            if (found.CurrentHp <= 0)
            {
                Debug.Log("enemy defeated, power up Onslaught");
                // gain AP and power up on next card use
                m_gainAp.Activate(context);
                m_powerUp = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}