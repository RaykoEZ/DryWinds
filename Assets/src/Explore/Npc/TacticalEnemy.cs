using Curry.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Curry.Explore
{
    // countdown: can be negative
    public delegate void OnEnemyCountdownUpdate(int countdown, Action onInterrupt = null);
    public delegate void OnEnemyUpdate(TacticalEnemy enemy);
    // Base enemy class
    public abstract class TacticalEnemy : MonoBehaviour, IEnemy, IPoolable
    {
        [SerializeField] protected TacticalStats m_initStats = default;
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] TextMeshPro m_countdownText = default;
        [SerializeField] AdventurerDetector m_detect = default;

        public event OnEnemyCountdownUpdate OnCountdownUpdate;
        public event OnEnemyUpdate OnActivate;
        public event OnEnemyUpdate OnStandby;
        public event OnEnemyUpdate OnDefeat;
        protected HashSet<Adventurer> m_targetsInSight = new HashSet<Adventurer>();
        protected int m_countdown;
        protected TacticalStats m_current;
        public TacticalStats InitStatus { get { return m_initStats; } }
        public TacticalStats CurrentStatus
        {
            get { return m_current; }
            protected set { m_current = value; }
        }
        public IObjectPool Origin { get; set; }

        public virtual void Prepare()
        {
            m_current = m_initStats;
            m_countdown = m_current.AttackCountdown;
            m_detect.OnDetected += OnDetectEnter;
            m_detect.OnExitDetection += OnDetectExit;
        }

        public virtual void ReturnToPool()
        {
            OnCountdownUpdate = null;
            OnActivate = null;
            OnStandby = null;
            OnDefeat = null;
            m_detect.OnDetected -= OnDetectEnter;
            m_detect.OnExitDetection -= OnDetectExit;
            Origin?.Reclaim(this);
        }

        protected virtual void Awake()
        {
            if (Origin == null)
            {
                Prepare();
            }
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
        public virtual void Affect(Func<TacticalStats, TacticalStats> effect)
        {
            if (effect == null) return;
            CurrentStatus = effect.Invoke(CurrentStatus);
        }
        public virtual void TakeHit()
        {
            Debug.Log("Ahh, me ded");
            m_anim?.SetTrigger("takeHit");
            m_anim?.SetBool("defeat", true);
        }
        protected virtual void ExecuteAction()
        {
            m_anim?.SetTrigger("strike");
        }
        protected virtual void OnCombat()
        {
            m_anim?.SetTrigger("engage");
            StartAttackCountdown();
        }
        protected virtual void OnDetectReaction()
        {
            m_anim?.SetTrigger("detect");
            OnActivate?.Invoke(this);
        }
        protected virtual void Standby()
        {
            //reset anim state and countdown
            m_anim?.SetTrigger("cancel");
            m_countdown = m_current.AttackCountdown;
            m_countdownText.text = "";
            OnStandby?.Invoke(this);
        }
        protected virtual void Defeat()
        {
            OnDefeat?.Invoke(this);
        }
        protected virtual void StartAttackCountdown()
        {
            m_countdown = m_current.AttackCountdown;
            m_countdownText.text = m_countdown.ToString();
        }
        public virtual void StandbyBehaviour()
        {
            if (m_targetsInSight.Count > 0)
            {
                OnDetectReaction();
                OnCombat();
            }
        }
        // countdown updates whenever tme is spent 
        public virtual IEnumerator CountdownTick(int dt)
        {
            for (int i = 0; i < dt; ++i)
            {
                m_countdown--;
                m_countdownText.text = Mathf.Max(m_countdown, 0).ToString();
                yield return new WaitForSeconds(0.1f);
            }
            OnCountdownUpdate?.Invoke(m_countdown, ExecuteAction);
        }

        void OnDetectEnter(Adventurer adv)
        {
            m_targetsInSight.Add(adv);
        }
        void OnDetectExit(Adventurer adv)
        {
            if (m_targetsInSight.Remove(adv))
            {
                // Do some animation here
                Standby();
            }
        }
    }

}
