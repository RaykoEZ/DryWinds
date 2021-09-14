using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public class Player : BaseCharacter
    {
        protected Camera m_cam = default;

        public Camera CurrentCamera { get { return m_cam; } }

        protected virtual void Update()
        {
            OnSPRegen();
        }

        public override void Init(CharacterContextFactory contextFactory)
        {
            base.Init(contextFactory);
            m_cam = Camera.main;
        }

        protected void OnSPRegen() 
        {
            m_statsManager.GainSp(Time.deltaTime * CurrentStats.SPRegenPerSec);
        }

    }
}
