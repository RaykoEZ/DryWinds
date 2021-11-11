using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Collection;

namespace Curry.Game
{
    public class Player : BaseCharacter
    {
        protected Camera m_cam = default;
        protected HeldInventory m_heldItems = new HeldInventory();
        public Camera CurrentCamera { get { return m_cam; } }
        public HeldInventory HeldInventory { get { return m_heldItems; } }

        public override void Init(CharacterContextFactory contextFactory)
        {
            base.Init(contextFactory);
            m_cam = Camera.main;
        }

        public void OnCollectItem(ICollectable item)
        {
            if (HeldInventory.Add(item))
            {
                //prompt successful
            }
            else
            {
                //prompt failure
            }
        }

        public void OnInteractPrompt(Action onClick)
        {
            //
        }
    }
}
