using System;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnEnemyUpdate(IEnemy enemy);
    public interface IEnemy : ICharacter
    {
        EnemyId Id { get; }
        TacticalStats InitStatus { get; }
        TacticalStats CurrentStatus { get; }
        Vector3 WorldPosition { get; }
        event OnEnemyUpdate OnDefeat;
        event OnEnemyUpdate OnReveal;
        event OnEnemyUpdate OnHide;
        void ExecuteAction();
        void OnDefeated();
        void Affect(Func<TacticalStats, TacticalStats> effect);
        bool UpdateCountdown(int dt);
    }

}
