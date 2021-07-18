using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class MovementHandler
    {
        public virtual void Move(Rigidbody2D rb, float speed, Vector2 dir) 
        {
            rb?.AddForce(dir * speed, ForceMode2D.Force);
        }
        public virtual void Dash(Rigidbody2D rb, float speed, Vector2 dir)
        {
            rb?.AddForce(dir * speed, ForceMode2D.Impulse);
        }
    }
}
