using Curry.Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public interface ISummonModule
    {
        void ApplyEffect(Vector3 targetTileWorldPos, PoolableBehaviour spawnRef, Action<PoolableBehaviour> onInstance = null);
    }
}
