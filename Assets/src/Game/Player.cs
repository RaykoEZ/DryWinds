using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public class Player : BaseCharacter
    {
        protected float m_spRegenTimer = 0f;
        protected Camera m_cam = default;

        public Camera CurrentCamera { get { return m_cam; } }

        protected virtual void Update()
        {
            OnSPRegen();
        }

        public void Init(CharacterContextFactory contextFactory)
        {
            m_statsManager.Init(contextFactory);
            m_cam = Camera.main;
        }

        public void Shutdown() 
        {
            m_statsManager.Shutdown();
        }

        public override void OnDefeat()
        {
        }

        protected void OnSPRegen() 
        {
            m_spRegenTimer += Time.deltaTime;
            if (CurrentStats.SP < CurrentStats.MaxSP) 
            {
                CurrentStats.SP = 
                    Mathf.Min(
                        CurrentStats.MaxSP,
                        CurrentStats.SP + m_spRegenTimer * CurrentStats.SPRegenPerSec);
                m_spRegenTimer = 0f;
            }
        }

    }
}
