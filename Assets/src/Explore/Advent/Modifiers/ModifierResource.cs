using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.VFX;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Mod_", menuName = "Curry/Modifiers/New Modifier Content", order = 1)]
    public class ModifierResource : ScriptableObject 
    {
        [SerializeField] ModifierContent m_content = default;
        [SerializeField] VisualEffectAsset m_vfx = default;
        [SerializeField] TimelineAsset m_vfxTimeline = default;
        public virtual ModifierContent Content => m_content;
        public VisualEffectAsset Vfx => m_vfx;
        public TimelineAsset VfxTimeline => m_vfxTimeline;
    }
}
