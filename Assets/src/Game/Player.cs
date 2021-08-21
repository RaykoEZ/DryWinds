using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Curry.Skill;

namespace Curry.Game
{
    public delegate void OnPlayerTakeDamage(float damage);

    public class Player : BaseCharacter
    {
        [SerializeField] protected PlayerContext m_playerContext = default;
        [SerializeField] BaseTracerBrush m_brush = default;

        protected int m_currentTraceIndex = 0;
        protected float m_spRegenTimer = 0f;
        protected PlayerContextFactory m_playerContextFactory = default;
        protected Camera m_cam = default;

        public event OnPlayerTakeDamage OnHitStun;

        public Camera CurrentCamera { get { return m_cam; } }
        public BaseTracerBrush CurrentBrush { get { return m_brush; } }
        public override CharacterStats CurrentStats { get { return m_playerContext.CharacterStats; } }
        public override CollisionStats CollisionStats { get { return m_playerContext.CurrentCollisionStats; } }

        protected virtual void Update()
        {
            if (m_playerContext.IsDirty) 
            {
                m_playerContextFactory.UpdateContext(m_playerContext);
            }

            OnSPRegen();
        }

        public void Init(PlayerContextFactory contextFactory, Camera cam)
        {
            m_playerContextFactory = contextFactory;
            m_playerContextFactory.UpdateContext(m_playerContext);
            m_playerContextFactory.Listen(OnPlayerContextUpdate);
            m_brush.EquipTracer(m_playerContext.EquippedTrace);
            m_cam = cam;
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
                m_brush.EquipTracer(m_playerContext.EquippedTrace);
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
            base.OnTakeDamage(damage);
            OnHitStun?.Invoke(damage);
        }

        public override void OnDefeat()
        {
        }

        protected void OnSPRegen() 
        {
            m_spRegenTimer += Time.deltaTime;
            if (m_playerContext.CharacterStats.SP < m_playerContext.CharacterStats.MaxSP) 
            {
                m_playerContext.CharacterStats.SP = 
                    Mathf.Min(
                        m_playerContext.CharacterStats.MaxSP,
                        m_playerContext.CharacterStats.SP + m_spRegenTimer * m_playerContext.CharacterStats.SPRegenPerSec);
                m_spRegenTimer = 0f;
            }
        }

        void OnPlayerContextUpdate(PlayerContext c) 
        {
            m_playerContext = c;
        }
    }
}
