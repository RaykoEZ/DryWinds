using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnEnemyUpdate(IEnemy enemy);
    public interface IEnemy : ICharacter
    {
        EnemyId Id { get; }
        TacticalStats InitStatus { get; }
        TacticalStats CurrentStatus { get; }
        IEnumerator ExecuteAction { get; }

        event OnEnemyUpdate OnDefeat;
        event OnEnemyUpdate OnReveal;
        event OnEnemyUpdate OnHide;
        void Affect(Func<TacticalStats, TacticalStats> effect);
        bool UpdateCountdown(int dt);
    }

}
