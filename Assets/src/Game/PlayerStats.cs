using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public class PlayerStats
    {
        [SerializeField] float m_hp;
        [SerializeField] float m_sp;
        [SerializeField] float m_spRegenPerSec;
        [SerializeField] float m_moveSpeed;
        bool m_isDirty = false;

        public bool IsDirty { get { return m_isDirty; } }

        public float HP { get { return m_hp; } set { m_hp = value; m_isDirty = true; } }
        public float SP { get { return m_sp; } set { m_sp = value; m_isDirty = true; } }
        public float SPRegenPerSec { get { return m_spRegenPerSec; } set { m_spRegenPerSec = value; m_isDirty = true; } }
        public float MoveSpeed { get { return m_moveSpeed; } set { m_moveSpeed = value; m_isDirty = true; } }

        public PlayerStats(float hp, float sp, float spPerSec, float moveSpeed) 
        {
            m_hp = hp;
            m_sp = sp;
            m_spRegenPerSec = spPerSec;
            m_moveSpeed = moveSpeed;
        }
    }
}
