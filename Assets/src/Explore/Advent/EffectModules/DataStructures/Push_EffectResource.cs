using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class Push_EffectResource : BaseEffectResource
    {
        [SerializeField] Push m_push = default;
        public Push PushModule => m_push;
    }
}
