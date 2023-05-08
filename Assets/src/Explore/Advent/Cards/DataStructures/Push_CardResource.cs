using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class Push_CardResource : BaseCardResource 
    {
        [SerializeField] Push_EffectResource m_push = default;
        public Push PushModule => m_push.PushModule;
    }
}
