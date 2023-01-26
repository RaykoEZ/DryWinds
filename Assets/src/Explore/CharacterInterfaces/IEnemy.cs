using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public struct EnemyUpdateContext 
    { 
    
    }
    public delegate void OnEnemyUpdate(IEnemy enemy);
    public interface IEnemy : ICharacter
    {
        bool SpotsTarget { get; }
        EnemyId Id { get; }
        TacticalStats InitStatus { get; }
        TacticalStats CurrentStatus { get; }
        event OnEnemyUpdate OnDefeat;
        event OnEnemyUpdate OnReveal;
        event OnEnemyUpdate OnHide;
        bool OnAction(int dt, bool reaction, out IEnumerator action);
    }

}
