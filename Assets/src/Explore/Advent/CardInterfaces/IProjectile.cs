using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public interface IProjectile  
    { 
        List<ITargetEffectModule> OnHitEffect { get; }
        void Fire(Vector3 targetPos);
        void Stop();
        void Deflect(Vector3 deflectDirection);
    }
}
