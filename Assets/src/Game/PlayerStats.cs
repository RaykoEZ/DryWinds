using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public class PlayerStats : CharacterStats
    {
        [SerializeField] protected float m_spRegenPerSec = default;

        public float SPRegenPerSec { get { return m_spRegenPerSec; } set { m_spRegenPerSec = value; m_isDirty = true; } }
        
        public PlayerStats(float stamina, float sp, float spPerSec)
        {
            m_stamina = stamina;
            m_sp = sp;
            m_spRegenPerSec = spPerSec;
        }
    }
}
