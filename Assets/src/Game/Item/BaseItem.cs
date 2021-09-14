using UnityEngine;

namespace Curry.Game
{
    public abstract class BaseItem : Interactable
    {
        [SerializeField] protected string m_itemName = default;
        [SerializeField] protected string m_description = default;

        protected BaseCharacter m_target;

        public virtual string ItemName { get { return m_itemName; } }
        public virtual string Description { get { return m_description; } }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            BaseCharacter hit = col.gameObject.GetComponent<BaseCharacter>();
            if (hit == null || hit.Relations == ObjectRelations.None)
            {
                return;
            }

            m_target = hit;
            if (ActivateEffect(hit)) 
            {
                OnConsumed();
            }
        }

        // returns true if item is expires after use
        public abstract bool ActivateEffect(BaseCharacter hit);

        public virtual void OnConsumed() 
        {
            Destroy(gameObject);
        }
    }
}
