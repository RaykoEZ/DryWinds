using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Curry.Explore
{
    [Serializable]
    public class AreaAttack : PropertyAttribute
    {
        [SerializeField] bool m_uniqueTargets = default;
        [SerializeField] int m_numToTarget = default;
        [SerializeField] RangeMapAsset m_attackRange = default;
        [SerializeField] DealDamage_EffectResource m_dealDamage = default;
        [SerializeField] protected LayerMask m_targetLayer = default;
        public IEnumerator ApplyEffect(ICharacter user)
        {
            if (GameUtil.HasValidTargets(user.WorldPosition, m_attackRange.Range, m_targetLayer, out List<ICharacter> validTargets))
            {
                List<ICharacter> result = SamplingUtil.SampleFromList(validTargets, m_numToTarget, m_uniqueTargets);
                foreach (var chosen in result)
                {
                    m_dealDamage.DamageModule?.ApplyEffect(chosen, user);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}