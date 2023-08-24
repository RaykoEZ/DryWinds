using Curry.Util;
using Curry.Vfx;
using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.VFX;

namespace Curry.Explore
{
    [Serializable]
    public abstract class BaseAbility : MonoBehaviour
    {
        [SerializeField] protected AbilityResource m_resource = default;
        public virtual AbilityContent AbilityDetail => m_resource.Content;
        public virtual VisualEffectAsset Vfx => m_resource.Vfx;
        public virtual TimelineAsset VfxTimeline => m_resource.VfxTimeline;
    }
}
