using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnEnemyMove(IEnemy moving, Vector3 destination, Action<Vector3> moveCall);
    public interface IEnemy : ICharacter
    {
        event OnEnemyMove OnMove;
        bool SpotsTarget { get; }
        EnemyId Id { get; }
        bool OnAction(int dt, bool reaction, out IEnumerator action);
    }
}
