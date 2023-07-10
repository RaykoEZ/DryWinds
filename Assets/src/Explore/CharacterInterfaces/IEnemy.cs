using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnEnemyMove(IEnemy moving, Vector3 destination, Action<Vector3> moveCall);
    public interface IEnemy : ICharacter
    {
        bool SpotsTarget { get; }
        EnemyId Id { get; }
        AbilityContent IntendingAbility { get; }
        bool OnAction(ActionCost dt, bool reaction, out IEnumerator action);
    }
    public delegate void OnMovementBlocked(Vector3 blockedWorldPos);
    public interface IMovable 
    {
        event OnMovementBlocked OnBlocked;
        void Move(Vector3 target);
    }
    public interface IMovableEnemy : IMovable
    {
        event OnEnemyMove OnMove;
    }
}
