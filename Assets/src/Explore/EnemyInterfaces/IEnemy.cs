using System;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnEnemyUpdate(IEnemy enemy);
    public interface IEnemy
    {
        EnemyId Id { get; }
        TacticalStats InitStatus { get; }
        TacticalStats CurrentStatus { get; }
        event OnEnemyUpdate OnDefeat;
        void ExecuteAction();
        void Reveal();
        void Hide();
        void TakeHit();
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
        [Range(0, 23)]
        [SerializeField] int m_activeFrom = default;
        [Range(0, 23)]
        [SerializeField] int m_activeUntil = default;
        public int ActiveFrom { get { return m_activeFrom; } }
        public int ActiveUntil { get { return m_activeUntil; } }

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
