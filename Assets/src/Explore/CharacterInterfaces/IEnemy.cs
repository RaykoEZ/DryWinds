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
        IEnumerator BasicAction { get; }
        IEnumerator Reaction { get; }
        event OnEnemyUpdate OnDefeat;
        event OnEnemyUpdate OnReveal;
        event OnEnemyUpdate OnHide;
        bool ChooseAction(int dt);

    }

}
