using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    [Serializable]
    public class PlayerContext : IGameContext
    {
        [SerializeField] PlayerStats m_baseStats;
        [SerializeField] PlayerStats m_currentStats;
        [SerializeField] TraceInventory m_traceInventory;
        [SerializeField] TraceAsset m_currentTrace;
        bool m_isDirty = false;

        public bool IsDirty { get { return m_isDirty || BaseStats.IsDirty || CurrentStats.IsDirty; } }

        #region Player Stats Properties
        public PlayerStats BaseStats
        {
            get
            {
                return m_baseStats;
            }
            set
            {
                m_baseStats = value;
                m_isDirty = true;
            }
        }
        public PlayerStats CurrentStats
        {
            get
            {
                return m_currentStats;
            }
            set
            {
                m_currentStats = value;
                m_isDirty = true;
            }
        }
        public TraceInventory TraceInventory
        {
            get
            {
                return m_traceInventory;
            }
            set
            {
                m_traceInventory = value;
                m_isDirty = true;
            }
        }
        public TraceAsset CurrentTrace
        {
            get
            {
                return m_currentTrace;
            }
            set
            {
                m_currentTrace = value;
                m_isDirty = true;
            }
        }
        #endregion

        public PlayerContext(PlayerStats baseStats, PlayerStats currentStats, TraceInventory inventory, TraceAsset trace) 
        {
            BaseStats = baseStats;
            CurrentStats = currentStats;
            TraceInventory = inventory;
            CurrentTrace = trace;
        }
    }

    public delegate void OnPlayerContextUpdate(PlayerContext c);
    public class PlayerContextFactory : IGameContextFactory<PlayerContext> 
    {
        PlayerContext m_context;
        OnPlayerContextUpdate m_onContextUpdate = default;

        public void UpdateContext(PlayerContext context)
        {
            m_context = context;
            m_onContextUpdate?.Invoke(m_context);
        }

        public void Listen(OnPlayerContextUpdate callback) 
        {
            m_onContextUpdate += callback;
        }

        public void Unlisten(OnPlayerContextUpdate callback) 
        {
            m_onContextUpdate -= callback;

        }

        public PlayerContext Context()
        {
            return m_context;
        }
    }


}
