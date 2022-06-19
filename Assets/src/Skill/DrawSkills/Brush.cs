using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Curry.Skill
{
    [Serializable]
    public class Brush : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer m_brushSprite = default;
        [SerializeField] protected CircleCollider2D m_brushCollider = default;
        public LayerMask CollisionFilter = default;
        public Rigidbody2D rb = default;
        bool m_isBlocking;
        public bool IsBlocked 
        { 
            get { return m_isBlocking; } 
            protected set { m_isBlocking = value; } 
        }

        void Awake()
        {
            m_isBlocking = false;
        }
        public void Init()
        {
            rb.position = Camera.main.ScreenToWorldPoint(
                Mouse.current.position.ReadValue());
            m_brushCollider.enabled = true;
            m_brushSprite.enabled = true;
        }
        public void Show(Vector2 target)
        {
            Follow(target);
            m_brushCollider.enabled = true;
            m_brushSprite.enabled = true;
        }

        public void Hide() 
        {
            m_brushCollider.enabled = false;
            m_brushSprite.enabled = false;
        }

        void Follow(Vector2 target) 
        {
            float dist = Vector2.Distance(target, rb.position);
            Vector2 dir = target - rb.position;
            RaycastHit2D hit = Physics2D.CircleCast(
                    rb.position,
                    0.2f * m_brushCollider.radius,
                    dir.normalized,
                    dist,
                    CollisionFilter);
            IsBlocked = hit;
            Debug.DrawLine(rb.position, target, Color.red);
            if (!IsBlocked)
            {
                StopAllCoroutines();
                StartCoroutine(MoveBrush(target, dir));
            }     
        }

        IEnumerator MoveBrush(Vector2 target, Vector2 dir) 
        {
            float duration = 0.1f;
            float t = 0f;
            Vector2 p;
            while(t < duration) 
            {
                t += Time.deltaTime;
                p = Vector2.Lerp(rb.position, target, t/duration);
                rb.MovePosition(p);
                yield return new WaitForFixedUpdate();
            }
            
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            IsBlocked = true;
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 dir = contact.normal;
            Vector2 diff = rb.position - dir;
            rb.MovePosition(rb.position + diff);
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            IsBlocked = false;
        }
    }
}
