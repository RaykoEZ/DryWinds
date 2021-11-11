using System;
using UnityEngine;

namespace Curry.Game
{
    public abstract class BaseItem : Interactable, ICollectable
    {
        [SerializeField] protected EntityProperty m_itemProperty = default;

        public virtual GameObject CollectedObject 
        { 
            get 
            { 
                return gameObject; 
            } 
        }

        public EntityProperty Property
        {
            get { return m_itemProperty; }
        }

        protected Player m_owner;

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            Player hit = col.gameObject.GetComponent<Player>();
            if (hit == null || hit.Relations == ObjectRelations.None)
            {
                return;
            }
            OnCloseBy(hit);
        }

        // On player entering vicinity, prompt for collection/interaction with player UI
        public virtual void OnCloseBy(Player player) 
        {
            Action onClick = () =>
            {
                m_owner = player;
                player.OnCollectItem(this);
            };
            player.OnInteractPrompt(onClick);
        }

        public virtual void Use()
        {
            if (OnActivate(m_owner))
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

        public void Discard()
        {
            Debug.Log("Discarded me.");
            OnConsumed();
        }
    }
}
