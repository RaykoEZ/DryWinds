using System;
using UnityEngine;
using Curry.Game;
namespace Curry.Explore
{
    [Serializable]
    public class AdventPoolCollection : PoolCollection<AdventCard> 
    {      
    }

    [Serializable]
    public class AdventInstanceManager : InstanceManager<AdventCard> 
    {
        [SerializeField] protected AdventPoolCollection m_pool = default;
        protected override PoolCollection<AdventCard> Pool { get { return m_pool; }}
    }
}
