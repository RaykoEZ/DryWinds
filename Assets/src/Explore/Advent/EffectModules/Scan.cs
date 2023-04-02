using UnityEngine;
using Curry.Events;
using System;

namespace Curry.Explore
{
    [Serializable]
    public class Scan : PropertyAttribute, ICharacterEffectModule
    {
        [Range(0, 3)]
        [SerializeField] int m_detectionLevel = default;
        [SerializeField] CurryGameEventTrigger m_onScan = default;

        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            ScanInfo info = new ScanInfo(user, m_detectionLevel, 3f, target.WorldPosition);
            m_onScan?.TriggerEvent(info);
        }
    }
}
