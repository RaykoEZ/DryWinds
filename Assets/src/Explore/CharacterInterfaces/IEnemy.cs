using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public interface IEnemy : ICharacter
    {
        bool SpotsTarget { get; }
        EnemyId Id { get; }
        bool OnAction(int dt, bool reaction, out IEnumerator action);
    }

}
