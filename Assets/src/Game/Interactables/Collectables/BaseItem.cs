using System;
using UnityEngine;
using Curry.UI;

namespace Curry.Game
{
    public abstract class BaseItem : Interactable, ICollectable
    {
        [SerializeField] protected EntityProperty m_itemProperty = default;
        protected delegate void OnPlayerLeaveItemRange();
        protected event OnPlayerLeaveItemRange OnLeaveRange;
        protected Player m_owner;
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
        // returns true if item is expires after use
        protected abstract bool OnActivate(BaseCharacter hit);

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            Player hit = col.gameObject.GetComponent<Player>();
            if (hit == null || hit.Relations == ObjectRelations.None)
            {
                return;
            }
            OnCloseBy(hit);
        }

        protected virtual void OnTriggerExit2D(Collider2D col)
        {
            Player hit = col.gameObject.GetComponent<Player>();
            if (hit == null || hit.Relations == ObjectRelations.None)
            {
                return;
            }
            OnLeaveRange?.Invoke();
            OnLeaveRange = null;
        }

        // On player entering vicinity, prompt for collection/interaction with player UI
        public virtual void OnCloseBy(Player player) 
        {
            Action onClick = () =>
            {
                m_owner = player;
                player.OnCollectItem(this);
            };

            InteractPrompt prompt = 
                player.OnInteractPrompt(onClick, Property.Name, type: EPromptType.Collect);
            prompt.Show();
            OnLeaveRange += ()=>{ prompt.Hide(); };
        }

        public virtual void Activate()
        {
            if (OnActivate(m_owner))
            {
                OnConsumed();
            }
        }

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
