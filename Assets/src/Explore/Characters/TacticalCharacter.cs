using Curry.Game;
using System.Collections;
using UnityEngine;
using UnityEditor;
namespace Curry.Explore
{
    public abstract class TacticalCharacter : PoolableBehaviour, ICharacter
    {
        [SerializeField] string m_name = default;
        [Range(1, 100)]
        [SerializeField] int m_maxHp = default;
        protected int m_currentHp = 1;
        // A list of layer names to check when we intend to move towards a position 
        static readonly string[] c_occupanceCheckFilter = new string[]
        {
            "Obstacles",
            "Enemies",
            "Player"
        };
        protected static LayerMask OccupanceContactFilter => LayerMask.GetMask(c_occupanceCheckFilter);
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

        public event OnMovementBlocked OnBlocked;
        public override void Prepare()
        {
            CurrentHp = MaxHp;
        }
        public abstract void Hide();
        public virtual void Move(Vector2Int direction)
        {
            Vector3 target = transform.position + new Vector3(direction.x, direction.y, 0f);
            RaycastHit2D hit = Physics2D.CircleCast(
                    target,
                    0.1f,
                    Vector2.zero,
                    distance: 0f,
                    OccupanceContactFilter);
            // Check for walls and occupying entities
            if (!hit)
            {
                StartCoroutine(Move_Internal(target));
            }
            else if(hit.rigidbody.TryGetComponent(out IStepOnTrigger steppedOn))
            {
                StartCoroutine(Move_Internal(target));
                steppedOn.Trigger(this);
            }
            else 
            {
                OnMovementBlocked(hit);
            }
        }
        void OnMovementBlocked(RaycastHit2D hit) 
        {
            Rigidbody2D rb = hit.rigidbody;
            if(rb == null)
            {
                OnBlocked?.Invoke(hit.point);
            }
            else if (rb.TryGetComponent(out ICharacter character)) 
            {
                character.Reveal();
            }
        }

        public virtual void OnDefeated()
        {
            ReturnToPool();
        }
        public virtual void Recover(int val)
        {
            Debug.Log("Player recovers" + val + " HP.");
            CurrentHp += val;
        }
        public abstract void Reveal();
        public abstract void TakeHit(int hitVal);
        protected virtual IEnumerator Move_Internal(Vector3 target)
        {          
            float duration = 1f;
            float timeElapsed = 0f;
            while (timeElapsed <= duration)
            {
                timeElapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, target, timeElapsed / duration);
                yield return null;
            }
            OnMoveFinish();
        }
        protected virtual void OnMoveFinish() { }
    }

}
