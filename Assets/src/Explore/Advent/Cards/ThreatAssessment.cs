using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class ThreatAssessment : CardResource, ICooldown
    {
        [SerializeField] Scan_EffectResource m_scan = default;

        public ThreatAssessment(ThreatAssessment effect) : base(effect)
        {
            m_scan = effect.m_scan;
        }

        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            m_scan?.ScanModule?.ApplyEffect(user, user);
            yield return new WaitForEndOfFrame();
        }
    }
}
