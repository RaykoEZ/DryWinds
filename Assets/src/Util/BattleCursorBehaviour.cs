using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Curry.Util
{
    // Not using for now
    public class BattleCursorBehaviour : MonoBehaviour
    {
        public Rigidbody2D rb = default;
        void FixedUpdate()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0.0f;
            rb.MovePosition(mousePos);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 dir = contact.normal;
            Vector2 diff = rb.position - dir;
            rb.MovePosition((rb.position + diff));

        }
    }
}

