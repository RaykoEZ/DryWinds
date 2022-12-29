using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class SceneInterruptCollection 
    {
        [SerializeField] protected List<SceneInterruptBehaviour> m_preLoadInterrupt = default;
        protected List<SceneInterruptBehaviour> m_interruptors;
        public IReadOnlyList<SceneInterruptBehaviour> SceneInterruptors => m_interruptors;
        public event SceneInterruptBehaviour.OnInputInterrupt OnInterruptBegin;
        public event SceneInterruptBehaviour.OnInputInterrupt OnInterruptEnd;

        public virtual void Init() 
        {
            m_interruptors = m_preLoadInterrupt;
            foreach(SceneInterruptBehaviour interrupt in m_interruptors) 
            {
                interrupt.OnInterruptBegin += StartInterrupt;
                interrupt.OnInterruptEnd += EndInterrupt;
            }
        }
        public void Add(SceneInterruptBehaviour interruptor) 
        {
            if (interruptor == null) return;
            interruptor.OnInterruptBegin += StartInterrupt;
            interruptor.OnInterruptEnd += EndInterrupt;
            m_interruptors.Add(interruptor);
        }
        public void Remove(SceneInterruptBehaviour interruptor) 
        {
            if (interruptor == null) return;
            if (m_interruptors.Remove(interruptor)) 
            {
                interruptor.OnInterruptBegin -= StartInterrupt;
                interruptor.OnInterruptEnd -= EndInterrupt;
            }
        }
        public void Clear() 
        {
            foreach (SceneInterruptBehaviour interrupt in m_interruptors)
            {
                interrupt.OnInterruptBegin -= StartInterrupt;
                interrupt.OnInterruptEnd -= EndInterrupt;
            }
            m_interruptors.Clear();
        }

        protected void StartInterrupt()
        {
            OnInterruptBegin?.Invoke();
        }
        protected void EndInterrupt()
        {
            OnInterruptEnd?.Invoke();
        }
    }
}