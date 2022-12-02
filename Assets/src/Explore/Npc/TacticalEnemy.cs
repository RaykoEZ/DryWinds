using Curry.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Curry.Explore
{
    // countdown: can be negative
    public delegate void OnEnemyCountdownUpdate(TacticalEnemy enemy);
    public delegate void OnEnemyUpdate(TacticalEnemy enemy);
    // Base enemy class
    public abstract class TacticalEnemy : MonoBehaviour, IEnemy, IPoolable
    {
        [SerializeField] protected TacticalStats m_initStats = default;
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] TextMeshPro m_countdownText = default;
        [SerializeField] AdventurerDetector m_detect = default;
        public event OnEnemyUpdate OnDefeat;
        protected HashSet<Adventurer> m_targetsInSight = new HashSet<Adventurer>();
        public int Countdown { get; protected set; }
        protected TacticalStats m_current;
        public virtual EnemyId Id { get; protected set; }
        public TacticalStats InitStatus { get { return m_initStats; } }
        public TacticalStats CurrentStatus
        {
            get { return m_current; }
            protected set { m_current = value; }
        }
        public IObjectPool Origin { get; set; }

        public virtual void Prepare()
        {
            // Get new id for enemy
            Id = new EnemyId(gameObject.name);
            m_current = m_initStats;
            Countdown = m_current.AttackCountdown;
            m_detect.OnDetected += OnDetectEnter;
            m_detect.OnExitDetection += OnDetectExit;
        }

        public virtual void ReturnToPool()
        {
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
        public virtual void OnDetect()
        {
            if (m_targetsInSight.Count > 0)
            {
                OnDetectReaction();
                OnCombat();
            }
        }

        public virtual bool UpdateCountdown(int dt)
        {
            if(m_targetsInSight.Count == 0) { return false; }
            StartCoroutine(CountdownTick(dt, Countdown));
            Countdown -= dt;
            bool countDownEnds = Countdown <= 0;
            return countDownEnds;
        }

        public virtual void ExecuteAction()
        {
            ResetCountdown();
            m_anim?.SetTrigger("strike");
        }
        protected virtual void OnCombat()
        {
            ResetCountdown();
            m_anim?.SetBool("combat", true);
        }
        protected virtual void OnDetectReaction()
        {
        }
        protected virtual void Standby()
        {
            //reset anim state and countdown
            m_anim?.SetBool("combat", false);
            Countdown = m_current.AttackCountdown;
            m_countdownText.text = "";
        }
        protected virtual void Defeat()
        {
            OnDefeat?.Invoke(this);
        }
        protected virtual void ResetCountdown()
        {
            Countdown = m_current.AttackCountdown;
            m_countdownText.text = Countdown.ToString();
        }

        // countdown text & fx update
        protected virtual IEnumerator CountdownTick(int dt, int startFrom)
        {
            for (int i = 0; i < dt; ++i)
            {
                startFrom--;
                m_countdownText.text = Mathf.Max(startFrom, 0).ToString();
                yield return new WaitForSeconds(0.1f);
            }
        }

        void OnDetectEnter(Adventurer adv)
        {
            m_targetsInSight.Add(adv);
            OnDetect();
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
