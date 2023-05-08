using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public interface IProjectile : IActiveAbility
    { 
        IEnumerator FireAt(Vector3 targetPos);
        void Stop();
        void Deflect(Vector3 deflectDirection);
    }
}
