using Curry.Game;
using System.Collections;
using UnityEngine;
using UnityEditor;
namespace Curry.Explore
{
    public abstract class TacticalCharacter : PoolableBehaviour, ICharacter
    {
        // A list of layer names to check when we intend to move towards a position 
        static readonly string[] c_occupanceCheckFilter = new string[]
        {
            "Obstacles",
            "Enemies",
            "Player"
        };
        protected static LayerMask OccupanceContactFilter => LayerMask.GetMask(c_occupanceCheckFilter);

        public abstract ObjectVisibility Visibility { get; protected set; }
        public event OnMovementBlocked OnBlocked;

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
        public abstract void Recover(int val);
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
