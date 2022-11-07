using System;
using System.Collections;
using UnityEngine;
using Curry.Events;
using TMPro;
namespace Curry.Explore
{
    // Base enemy class
    public abstract class TacticalEnemy: MonoBehaviour, IEnemy 
    {
        [SerializeField] protected TacticalStats m_initStats = default;
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] protected CurryGameEventListener m_onCountdownTick = default;
        [SerializeField] TextMeshPro m_countdownText = default;
        protected int m_countdown;
        protected TacticalStats m_current;
        public TacticalStats InitStatus { get { return m_initStats; } }
        public TacticalStats CurrentStatus { 
            get { return m_current; } 
            protected set { m_current = value; } }

        protected virtual void Awake()
        {
            m_current = m_initStats;
            m_countdown = m_current.AttackCountdown;
        }
        public virtual void Reveal() 
        {
            m_current.Visibility = TacticalVisibility.Visible;
            m_anim.SetBool("hidden", false);
        }
        public virtual void Hide() 
        {
            m_current.Visibility = TacticalVisibility.Hidden;
            m_anim.SetBool("hidden", true);
        }
        public virtual void TakeHit() 
        {
            Debug.Log("Ahh, me ded");
            m_anim?.SetTrigger("takeHit");
            m_anim?.SetBool("defeat", true);
        }
        protected virtual void OnCombat() 
        {
            m_anim?.SetTrigger("engage");
            StartAttackCountdown();
        }
        protected virtual void OnDetectReaction() 
        {
            m_anim?.SetTrigger("detect");
        }
        protected virtual void Strike() 
        {
            m_anim?.SetTrigger("strike");
            m_onCountdownTick?.Shutdown();
        }
        protected virtual void StartAttackCountdown() 
        {
            m_countdown = m_current.AttackCountdown;
            m_countdownText.text = m_countdown.ToString();
            // Start listening to time intervals for countdowns updates
            m_onCountdownTick?.Init();
        }

        // countdown updates whenever tme is spent 
        public void OnTimeSpent(EventInfo time)
        {
            if (time is TimeInfo spend)
            {
                StartCoroutine(CountdownTick(spend.Time));
            }
        }
        protected IEnumerator CountdownTick(int dt) 
        { 
            for (int i = 0; i < dt; ++i) 
            {
                m_countdown = Mathf.Max(m_countdown - 1, 0);
                m_countdownText.text = m_countdown.ToString();
                yield return new WaitForSeconds(0.5f);
            }

            if (m_countdown == 0) 
            {
                Strike();
            }
        }

        public virtual void Affect(Func<TacticalStats, TacticalStats> effect)
        {
            if (effect == null) return;
            CurrentStatus = effect.Invoke(CurrentStatus);
        }
    }

}
