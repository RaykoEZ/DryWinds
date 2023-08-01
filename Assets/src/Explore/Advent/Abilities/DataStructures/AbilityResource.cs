using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.VFX;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Ability_", menuName = "Curry/Ability/New Ability Resource", order = 1)]
    public class AbilityResource : ScriptableObject 
    {
        [SerializeField] protected AbilityContent m_content = default;
        // A vfx and director to insert into a ability instance for effect sequence when activating a card
        [SerializeField] protected VisualEffectAsset m_vfx = default;
        [SerializeField] protected TimelineAsset m_vfxTimeLine = default;
        public AbilityContent Content => m_content;
        public VisualEffectAsset Vfx => m_vfx;
        public TimelineAsset VfxTimeline => m_vfxTimeLine;
    }
}
