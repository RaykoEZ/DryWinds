using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game 
{
    public class CollisionHandler : MonoBehaviour
    {
        [SerializeField] Rigidbody2D Rigidbody = default;
        [SerializeField] float ImpulseModifier = 5f;
        void OnCollisionEnter2D(Collision2D collision)
        {
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 dir = contact.normal;

            Rigidbody.AddForce(ImpulseModifier * dir, ForceMode2D.Impulse);
        }
    }
}

