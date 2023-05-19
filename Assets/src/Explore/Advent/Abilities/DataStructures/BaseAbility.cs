﻿using Curry.Util;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public abstract class BaseAbility : MonoBehaviour
    {
        [SerializeField] protected AbilityResource m_resource = default;
        public virtual AbilityContent Content => m_resource.Content;
    }
}