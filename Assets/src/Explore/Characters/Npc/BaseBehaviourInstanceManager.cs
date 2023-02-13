using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    [Serializable]
    public class BaseBehaviourPoolCollection : PoolCollection<PoolableBehaviour>
    {
    }
    [Serializable]
    public class BaseBehaviourInstanceManager : InstanceManager<PoolableBehaviour>
    {
        [SerializeField] BaseBehaviourPoolCollection m_pool = default;
        public Transform PoolDefaultParent => Pool.DefaultParent;
        protected override PoolCollection<PoolableBehaviour> Pool { get { return m_pool; } }
    }
}