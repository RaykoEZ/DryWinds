using Curry.Game;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.AI;

namespace Curry.Explore
{
    public abstract class TacticalCharacter : PoolableBehaviour, ICharacter
    {
        [SerializeField] protected string m_name = default;
        [Range(1, 100)]
        [SerializeField] protected int m_maxHp = default;
        [Range(0, 3)]
        [SerializeField] protected int m_moveRange = default;
        [SerializeField] protected CharacterNavigator m_navigator = default;
        protected int m_currentHp = 1;
        protected int m_currentmoveRange = 1;
        protected bool m_blocked = false;
        public virtual ObjectVisibility Visibility { get; protected set; } = ObjectVisibility.Visible;
        public Vector3 WorldPosition => transform.position;
        public string Name => m_name;
        public int MaxHp
        {
            get { return m_maxHp; }
            protected set { m_maxHp = Mathf.Clamp(value, 1, 100); }
        }
        public int CurrentHp { 
            get { return m_currentHp; } 
            protected set { m_currentHp = Mathf.Clamp(value, 0, MaxHp); } 
        }

        public int MoveRange { get { return m_currentmoveRange; } protected set { m_currentmoveRange = Mathf.Clamp(value, 0, 3); } }
        public event OnMovementBlocked OnBlocked;

        public override void Prepare()
        {
            CurrentHp = MaxHp;
            MoveRange = m_moveRange;
        }
        public abstract void Hide();
        public virtual void Move(Vector3 target)
        {
            StartCoroutine(Move_Internal(target));
        }
        public virtual void OnMovementBlocked(ICharacter blocking) 
        {
            m_blocked = true;
            m_navigator.StopMovement();
            Reveal();
            blocking.Reveal();          
        }

        public virtual void OnDefeated()
        {
            ReturnToPool();
        }
        public virtual void Recover(int val)
        {
            Debug.Log("Player recovers " + val + " HP.");
            CurrentHp += val;
        }
        public abstract void Reveal();
        public abstract void TakeHit(int hitVal);
        protected virtual IEnumerator Move_Internal(Vector3 target)
        {
            m_blocked = false;
            yield return StartCoroutine(m_navigator.MoveTo(target));
            float duration = 1f;
            float timeElapsed = 0f;
            while (timeElapsed <= duration)
            {
                if (m_blocked)
                {
                    OnBlocked?.Invoke(WorldPosition);
                    m_navigator.StopMovement();
                    break;
                }
                timeElapsed += Time.deltaTime;
                transform.position = Vector2.Lerp(transform.position, m_navigator.AgentPosition, timeElapsed / duration);
                yield return null;
            }
            OnMoveFinish();
        }
        protected virtual void OnMoveFinish() { }
        public bool Warp(Vector3 to)
        {
            int boundFilter = 1 << LayerMask.NameToLayer("Environment");
            Collider2D[] hit = Physics2D.OverlapCircleAll(
                to,
                0.1f, layerMask: boundFilter);
            // only warp to positions with terrain tiles (inside our level's bounds)
            if (hit.Length > 0) 
            {
                transform.position = to;
            }
            return hit.Length > 0;
        }

    }

}
