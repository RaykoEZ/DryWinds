using Curry.Util;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public struct AbilityContent
    {
        [SerializeField] public string Name;
        [TextArea]
        [SerializeField] public string Description;
        // This sprite will be a grid pattern
        [SerializeField] public RangeMap TargetingRange;
        [SerializeField] public Sprite Icon;
    }
}
