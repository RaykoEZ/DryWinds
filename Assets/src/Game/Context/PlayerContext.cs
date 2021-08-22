using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    [Serializable]
    public class PlayerContext : CharacterContext
    {
        [SerializeField] TraceInventory m_traceInventory = default;
        [SerializeField] TraceAsset m_equippedTrace = default;

        #region Player Stats Properties

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

        public PlayerContext(PlayerContext c) : base(c)
        {
            m_traceInventory = c.m_traceInventory;
            m_equippedTrace = c.m_equippedTrace;
        }

    }

    public delegate void OnPlayerContextUpdate(PlayerContext c);
    public class PlayerContextFactory : IGameContextFactory<PlayerContext> 
    {
        PlayerContext m_context;
        public event OnContextUpdate<PlayerContext> OnUpdate;

        public PlayerContext Context { get { return new PlayerContext(m_context); } }

        public void UpdateContext(PlayerContext context)
        {
            m_context =  new PlayerContext(context);
            OnUpdate?.Invoke(m_context);
        }
    }
}
