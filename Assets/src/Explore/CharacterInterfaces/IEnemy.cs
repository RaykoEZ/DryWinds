using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public class EnemyIntent
    {
        private AbilityContent m_ability;
        private IEnumerator m_call;
        public AbilityContent Ability { get => m_ability; set => m_ability = value; }
        public IEnumerator Call { get => m_call; set => m_call = value; }
        public EnemyIntent(AbilityContent ability, IEnumerator call)
        {
            Ability = ability;
            Call = call;
        }
        public static EnemyIntent None => new EnemyIntent(AbilityContent.None, null);
    }
    public delegate void OnEnemyMove(IEnemy moving, Vector3 destination, Action<Vector3> moveCall);
    public delegate void OnAbilityMessage(string message);
    public interface IEnemy : ICharacter
    {
        event OnAbilityMessage OnAbility;
        bool SpotsTarget { get; }
        EnemyId Id { get; }
        EnemyIntent IntendingAction { get; }
        bool UpdateAction(ActionCost dt, out EnemyIntent action);
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
