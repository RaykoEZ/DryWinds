using System;
using UnityEngine;

namespace Curry.Game
{
    public abstract class BaseItem : Interactable, ICollectable
    {
        [SerializeField] protected string m_itemName = default;
        [SerializeField] protected string m_description = default;
        [SerializeField] protected Sprite m_sprite = default;
        public virtual string Name { get { return m_itemName; } }
        public virtual string Description { get { return m_description; } }
        public virtual GameObject CollectedObject { get { return gameObject; } }
        public virtual Sprite ItemSprite { get { return m_sprite; } }

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

        public virtual void UseItem()
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
    }
}
