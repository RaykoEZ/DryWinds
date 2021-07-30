using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Curry.Skill;
using Curry.Events;

namespace Curry.Game
{
    public class Player : Interactable
    {
        [SerializeField] protected PlayerContext m_playerContext = default;
        [SerializeField] BaseTracerBrush m_brush = default;

        protected int m_currentTraceIndex = 0;
        protected float m_spRegenTimer = 0f;
        protected PlayerContextFactory m_playerContextFactory = default;

        public BaseTracerBrush CurrentBrush { get { return m_brush; } }
        public PlayerStats Stats { get { return m_playerContext.PlayerStats; } }
        public override CollisionStats CollisionStats { get { return m_playerContext.CurrentCollisionStats; } }

        protected virtual void Update()
        {
            if (m_playerContext.IsDirty) 
            {
                m_playerContextFactory.UpdateContext(m_playerContext);
            }

            OnSPRegen();
        }

        public void Init(PlayerContextFactory contextFactory)
        {
            m_playerContextFactory = contextFactory;
            m_playerContextFactory.UpdateContext(m_playerContext);
            m_playerContextFactory.Listen(OnPlayerContextUpdate);
            m_brush.EquipTrace(m_playerContext.EquippedTrace);
        }

        public void Shutdown() 
        {
            m_playerContextFactory.Unlisten(OnPlayerContextUpdate);
        }

        public void ChangeTrace(int index)
        {
            TraceAsset newTrace = m_playerContext.TraceInventory.GetTrace(index);
            if (newTrace != null)
            {
                m_playerContext.EquippedTrace = newTrace;
                m_currentTraceIndex = index;
                m_brush.EquipTrace(m_playerContext.EquippedTrace);
            }
        }

        public void NextTrace()
        {
            int newIndex = m_currentTraceIndex + 1;
            ChangeTrace(newIndex);
        }

        public void PreviousTrace()
        {
            int newIndex = m_currentTraceIndex - 1;
            ChangeTrace(newIndex);
        }

        public override void OnTakeDamage(float damage)
        {
            m_playerContext.PlayerStats.Stamina -= damage;

            if (m_playerContext.PlayerStats.Stamina <= 0f) 
            {
                OnDefeat();
            }
        }

        protected override void OnTouch(Interactable incomingInteraction)
        {
            if (incomingInteraction.Relations == ObjectRelations.Hostile)
            {
                incomingInteraction.OnTakeDamage(CollisionStats.ContactDamage);
            }
        }

        public override void OnDefeat()
        {
        }

        protected void OnSPRegen() 
        {
            m_spRegenTimer += Time.deltaTime;
            if (m_spRegenTimer >= 1.0f &&
                m_playerContext.PlayerStats.SP < m_playerContext.PlayerStats.MaxSP) 
            {
                m_spRegenTimer = 0f;
                m_playerContext.PlayerStats.SP = 
                    Mathf.Min(
                        m_playerContext.PlayerStats.MaxSP,
                        m_playerContext.PlayerStats.SP + m_playerContext.PlayerStats.SPRegenPerSec);
            }
        }

        void OnPlayerContextUpdate(PlayerContext c) 
        {
            m_playerContext = c;
        }
    }
}
