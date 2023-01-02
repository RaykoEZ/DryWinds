using System;
using UnityEngine;

namespace Curry.Explore
{
    public interface ICharacter 
    {
        void Reveal();
        void Hide();
        void TakeHit(int hitVal);
        void Move(Vector2Int direction);
    }
    public interface IPlayer : ICharacter
    {
    }
    public delegate void OnEnemyUpdate(IEnemy enemy);
    public interface IEnemy : ICharacter
    {
        EnemyId Id { get; }
        TacticalStats InitStatus { get; }
        TacticalStats CurrentStatus { get; }
        event OnEnemyUpdate OnDefeat;
        void ExecuteAction();
        void OnDefeated();
        void Affect(Func<TacticalStats, TacticalStats> effect);
        bool UpdateCountdown(int dt);
    }
    #region Active hours class body
    // Describes behaviours when time of day changes
    // If m_activeFrom == m_activeUntil:
    // character will be inactive at the same time next cycle/day
    [Serializable]
    public class ActiveTimeFrame
    {
        [SerializeField] GameClock.TimeOfDay m_activeAt = default;
        public GameClock.TimeOfDay ActiveAt { get { return m_activeAt; } }
    }
    #endregion
    public interface IOrganicLife 
    { 
        ActiveTimeFrame ActiveHours { get; }
        bool IsActive { get; }
        void Activate();
        void Hibernate();
    }
}
