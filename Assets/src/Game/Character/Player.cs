using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Collection;
using Curry.UI;

namespace Curry.Game
{
    public delegate void OnCollectItem(int slot, EntityProperty itemProperty, ICollectable item);
    public class Player : BaseCharacter
    {
        [SerializeField] PromptManager m_prompt = default;
        protected Camera m_cam = default;
        protected HeldInventory m_heldItems = new HeldInventory();
        public Camera CurrentCamera { get { return m_cam; } }
        public HeldInventory HeldInventory { get { return m_heldItems; } }
        
        public event OnCollectItem OnCollect;

        public override void Init(CharacterContextFactory contextFactory)
        {
            base.Init(contextFactory);
            m_cam = Camera.main;
        }

        public override void ReturnToPool()
        {
            OnCollect = null;
            base.ReturnToPool();
        }

        public void OnCollectItem(ICollectable item)
        {
            if (HeldInventory.Add(item, out int slot))
            {
                //collect successful
                OnCollect?.Invoke(slot, item.Property, item);
            }
            else
            {
                //collect failure
                Debug.Log($"Cannot collect {item.Property.Name}.");
            }
        }

        public InteractPrompt OnInteractPrompt(Action onClick, string title, EPromptType type = EPromptType.Interact)
        {
            // Shoe prompt for interaction
            return m_prompt.GetPrompt(onClick, title, type);
        }
    }
}
