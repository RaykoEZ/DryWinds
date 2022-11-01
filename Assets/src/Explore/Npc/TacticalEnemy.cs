using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    // Base enemy class
    public abstract class TacticalEnemy: MonoBehaviour, IEnemy 
    {
        [SerializeField] protected TacticalStats m_initStats = default;
        [SerializeField] protected Animator m_anim = default;
        protected TacticalStats m_current;
        public TacticalStats InitStatus { get { return m_initStats; } }
        public TacticalStats CurrentStatus { 
            get { return m_current; } 
            protected set { m_current = value; } }

        void Awake()
        {
            m_current = m_initStats;    
        }
        public virtual void Reveal() 
        {
            m_current.Visibility = TacticalVisibility.Visible;
            m_anim.SetTrigger("reveal");
        }
        public virtual void Hide() 
        {
            m_current.Visibility = TacticalVisibility.Hidden;
            m_anim.SetTrigger("hide");
        }

        public virtual void TakeHit() 
        {
            Debug.Log("Ahh, me ded");
            m_anim?.SetTrigger("takeHit");
            m_anim?.SetBool("defeat", true);
        }
        protected virtual void OnCombat() 
        {
            m_anim?.SetBool("engage", true);

        }
        public virtual void Affect(Func<TacticalStats, TacticalStats> effect)
        {
            if (effect == null) return;
            CurrentStatus = effect.Invoke(CurrentStatus);
        }
    }

}
