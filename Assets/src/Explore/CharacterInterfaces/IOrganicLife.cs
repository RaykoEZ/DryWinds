using System;
using UnityEngine;
namespace Curry.Explore
{
    #region Active time frame class body
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
