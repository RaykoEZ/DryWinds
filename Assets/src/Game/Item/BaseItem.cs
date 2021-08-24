using UnityEngine;

namespace Curry.Game
{
    public abstract class BaseItem : Interactable
    {
        [SerializeField] protected string m_itemName = default;

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            BaseCharacter hit = col.gameObject.GetComponent<BaseCharacter>();
            if (hit == null || hit.Relations == ObjectRelations.None)
            {
                return;
            }

            if (ActivateEffect(hit)) 
            {
                OnExpire();
            }
        }

        // returns true if item is expires after use
        public abstract bool ActivateEffect(BaseCharacter hit);

        public virtual void OnExpire() 
        {
            Destroy(gameObject);
        }
    }
}
