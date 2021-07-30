using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Curry.Game
{
    public class MovementHandler : MonoBehaviour
    {
        [SerializeField] Rigidbody2D m_rigidbody = default;

        public virtual void Dash(float speed, float charge)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 dir = mousePos - m_rigidbody.position;
            float speedMod = speed * Mathf.Clamp(charge, 1.0f, 3.0f);
            m_rigidbody?.AddForce(dir.normalized * speedMod, ForceMode2D.Impulse);
        }
    }
}
