using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Events
{
    public class InteractableArgs : EventArgs
    {
        Interactable m_interactable;
        public Interactable Interactable { get { return m_interactable; } }

        public InteractableArgs(Interactable interactable)
        {
            m_interactable = interactable;
        }
    }
}
