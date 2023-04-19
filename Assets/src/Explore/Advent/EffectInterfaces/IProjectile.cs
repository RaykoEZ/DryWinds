using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public interface IProjectile  
    { 
        IReadOnlyList<ITargetEffectModule> OnHitEffects { get; }
        void AddOnHitEffect(ITargetEffectModule mod);
        IEnumerator FireAt(Vector3 targetPos);
        void Stop();
        void Deflect(Vector3 deflectDirection);
    }
}
