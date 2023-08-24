using UnityEngine;

namespace Curry.Game
{
    public abstract class PoolableBehaviour : MonoBehaviour, IPoolable
    {
        public IObjectPool Origin { get; set; }
        protected virtual void Awake()
        {
            if (Origin == null)
            {
                Prepare();
            }
        }
        public abstract void Prepare();

        public virtual void ReturnToPool()
        {
            if (Origin == null) 
            {
                Destroy(gameObject);
            } 
            else 
            {
                Origin.Reclaim(this);
            }
        }
    }
}
