using UnityEngine;

namespace Curry.Game
{
    public abstract class BaseItem : Interactable, IProximityPrompt, ICollectable
    {
        [SerializeField] protected string m_itemName = default;
        [SerializeField] protected string m_description = default;

        public virtual string Name { get { return m_itemName; } }
        public virtual string Description { get { return m_description; } }
        public virtual GameObject CollectedObject { get { return gameObject; } }
        
        protected BaseCharacter m_target;

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            BaseCharacter hit = col.gameObject.GetComponent<BaseCharacter>();
            if (hit == null || hit.Relations == ObjectRelations.None)
            {
                return;
            }

            m_target = hit;
            OnUse();
        }

        // On player entering vicinity, prompt for collection/interaction with player UI
        public virtual void OnProximity() 
        { 
            
        }

        public virtual void OnUse()
        {
            if (OnActivate(m_target))
            {
                OnConsumed();
            }
        }

        // returns true if item is expires after use
        public abstract bool OnActivate(BaseCharacter hit);

        public virtual void OnConsumed() 
        {
            Destroy(gameObject);
        }
    }
}
