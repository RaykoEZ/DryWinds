using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Collection;
using Curry.UI;
using Curry.Events;

namespace Curry.Game
{
    public delegate void OnCollectItem(int slot, EntityProperty itemProperty);
    public delegate void OnUseItem(int slot, bool expired);
    public class Player : BaseCharacter
    {
        [SerializeField] CurryGameEventSource m_onCollectItem = default;
        [SerializeField] PlayerHUDManager m_playerUI = default;
        [SerializeField] PromptManager m_prompt = default;
        protected Camera m_cam = default;
        protected HeldInventory m_heldItems = new HeldInventory();
        public Camera CurrentCamera { get { return m_cam; } }
        public HeldInventory Inventory { get { return m_heldItems; } }
        
        public event OnCollectItem OnCollect;
        public event OnUseItem OnUse;

        public override void Prepare()
        {
            base.Prepare();
            m_playerUI.Init(m_contextFactory, this);
            m_cam = Camera.main;
        }

        public override void ReturnToPool()
        {
            OnCollect = null;
            m_playerUI.Shutdown(m_contextFactory, this);
            base.ReturnToPool();
        }

        public void OnCollectItem(ICollectable item)
        {
            if (Inventory.Add(item, out int slot))
            {
                //collect successful
                OnCollect?.Invoke(slot, item.Property);
                Dictionary<string, object> payload = new Dictionary<string, object> { { "collected", item } };
                EventInfo info = new EventInfo(payload);
                m_onCollectItem?.Broadcast(info);
            }
            else
            {
                //collect failure
                Debug.Log($"Cannot collect {item.Property.Name}.");
            }
        }

        public void UseItem(int slot) 
        {
            Inventory.UseItem(slot, out bool expired);
            Debug.Log(expired);
            OnUse?.Invoke(slot, expired);
        }

        public InteractPrompt OnInteractPrompt(Action onClick, string title, EPromptType type = EPromptType.Interact)
        {
            // Shoe prompt for interaction
            return m_prompt.GetPrompt(onClick, title, type);
        }
    }
}
