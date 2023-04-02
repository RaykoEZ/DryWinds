using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public interface IProjectile  
    { 
        IReadOnlyList<ITargetEffectModule> OnHitEffects { get; }
        IEnumerator FireAt(Vector3 targetPos);
        void Stop();
        void Deflect(Vector3 deflectDirection);
    }
}
