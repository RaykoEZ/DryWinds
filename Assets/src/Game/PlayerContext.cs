using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    [Serializable]
    public class PlayerContext : IGameContext
    {
        [SerializeField] CollisionStats m_currentCollisionStats = default;
        [SerializeField] CharacterStats m_playerStats = default;
        [SerializeField] TraceInventory m_traceInventory = default;
        [SerializeField] TraceAsset m_equippedTrace = default;
        bool m_isDirty = false;

        public bool IsDirty { 
            get 
            {
                return m_isDirty ||
                    CharacterStats.IsDirty ||
                    m_currentCollisionStats.IsDirty;
            } 
        }

        #region Player Stats Properties
        public CollisionStats CurrentCollisionStats 
        { 
            get { return m_currentCollisionStats; } 
            set { m_currentCollisionStats = value; m_isDirty = true; } 
        }
        public CharacterStats CharacterStats
        {
            get
            {
                return m_playerStats;
            }
            set
            {
                m_playerStats = value;
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
        public TraceAsset EquippedTrace
        {
            get
            {
                return m_equippedTrace;
            }
            set
            {
                m_equippedTrace = value;
                m_isDirty = true;
            }
        }
        #endregion

        public PlayerContext(PlayerContext c) 
        {
            m_currentCollisionStats = new CollisionStats(c.m_currentCollisionStats);
            m_playerStats = new CharacterStats(c.m_playerStats);
            m_traceInventory = c.m_traceInventory;
            m_equippedTrace = c.m_equippedTrace;
        }

        public PlayerContext(
            CollisionStats baseCollisionStats,
            CharacterStats currentStats, 
            TraceInventory inventory, 
            TraceAsset trace) 
        {
            m_currentCollisionStats = baseCollisionStats;
            m_playerStats = currentStats;
            m_traceInventory = inventory;
            m_equippedTrace = trace;
        }
    }

    public delegate void OnPlayerContextUpdate(PlayerContext c);
    public class PlayerContextFactory : IGameContextFactory<PlayerContext> 
    {
        PlayerContext m_context;
        OnPlayerContextUpdate m_onContextUpdate = default;

        public void UpdateContext(PlayerContext context)
        {
            m_context =  new PlayerContext(context);
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
