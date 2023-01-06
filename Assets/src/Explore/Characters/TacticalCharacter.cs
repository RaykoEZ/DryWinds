using Curry.Game;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public abstract class TacticalCharacter : PoolableBehaviour, ICharacter
    {
        // A list of layer names to check when we intend to move towards a position 
        protected static readonly string[] c_occupanceCheckFilter = new string[] 
        {
            "Obstacles",
            "Enemies",
            "Player"
        };
        public abstract void Hide();

        public virtual void Move(Vector2Int direction)
        {
            Vector3 target = transform.position + new Vector3(direction.x, direction.y, 0f);
            RaycastHit2D hit = Physics2D.Linecast(
                    transform.position,
                    target,
                    LayerMask.GetMask(c_occupanceCheckFilter));
            // Check for walls and occupying entities
            if (!hit)
            {
                StartCoroutine(Move_Internal(target));
            }
            else 
            { 
                // TODO: reveal occupied tile, clear fog of war on that tile

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
