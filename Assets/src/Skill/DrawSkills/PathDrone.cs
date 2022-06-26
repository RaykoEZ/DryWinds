using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
namespace Curry.Skill
{
    public delegate void OnDroneMove(Vector2 pos);
    [Serializable]
    public class PathDrone : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer m_brushSprite = default;
        [SerializeField] protected CircleCollider2D m_brushCollider = default;
        public LayerMask CollisionFilter = default;
        public Rigidbody2D rb = default;
        public OnDroneMove OnMove;
        bool m_isBlocking;
        public bool IsBlocked 
        { 
            get { return m_isBlocking; } 
            protected set { m_isBlocking = value; } 
        }

        void Awake()
        {
            Init();
            m_isBlocking = false;
        }
        void Init()
        {
        }
        public void Show(Vector2 target)
        {
            float dist = Vector2.Distance(target, rb.position);
            if (dist > 0.5f)
            {
                Follow(target);
                m_brushCollider.enabled = true;
                m_brushSprite.enabled = true;
            }
        }

        public void Hide() 
        {
            m_brushCollider.enabled = false;
            m_brushSprite.enabled = false;
        }

        void Follow(Vector2 target) 
        {
            float duration = 0.25f;
            Vector2 dir = target - rb.position;
            Vector2 future = rb.position + (dir.normalized);
            Debug.DrawLine(rb.position, target, Color.red);
            IsBlocked = CollisionCheck(future);
            if (!IsBlocked)
            {
                StopAllCoroutines();
                StartCoroutine(MoveBrush(future, duration));
            }    
        }

        IEnumerator MoveBrush(Vector2 target, float duration = 1f) 
        {
            float t = 0f;
            Vector2 p;
            while (t < duration) 
            {
                float dist = Vector2.Distance(target, rb.position);
                if (dist > 0.5f)
                {
                    t += Time.deltaTime;
                    p = Vector2.Lerp(rb.position, target, t / duration);
                    rb.MovePosition(p);
                }
                else 
                {
                    yield break;
                }
                yield return new WaitForFixedUpdate();
                OnMove?.Invoke(rb.position);
            }
        }

        bool CollisionCheck(Vector2 target) 
        {
            float dist = Vector2.Distance(target, rb.position) + 0.1f;
            Vector2 dir = target - rb.position;
            RaycastHit2D hit = Physics2D.CircleCast(
            rb.position,
            0.2f * m_brushCollider.radius,
            dir.normalized,
            dist,
            CollisionFilter);
            return hit;

        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            IsBlocked = true;
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            IsBlocked = false;
        }
    }
}
