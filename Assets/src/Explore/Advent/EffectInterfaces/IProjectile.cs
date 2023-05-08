using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public interface IProjectile  
    { 
        IReadOnlyList<IStatusAilment> OnHitEffects { get; }
        void AddOnHitEffect(IStatusAilment mod);
        IEnumerator FireAt(Vector3 targetPos);
        void Stop();
        void Deflect(Vector3 deflectDirection);
    }
}
