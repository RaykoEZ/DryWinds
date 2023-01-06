using Curry.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Curry.Explore
{
    // Base enemy class
    public abstract class TacticalEnemy : TacticalCharacter, IEnemy, IPoolable
    {
        [SerializeField] protected TacticalStats m_initStats = default;
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] protected TextMeshPro m_countdownText = default;
        [SerializeField] protected PlayerDetector m_detect = default;
        protected HashSet<IPlayer> m_targetsInSight = new HashSet<IPlayer>();
        public int Countdown { get; protected set; }
        protected TacticalStats m_current;

        #region ICharacter & IEnemy interface 

        public event OnEnemyUpdate OnDefeat;
        public event OnEnemyUpdate OnReveal;
        public event OnEnemyUpdate OnHide;

        public virtual EnemyId Id { get; protected set; }
        public TacticalStats InitStatus { get { return m_initStats; } }
        public TacticalStats CurrentStatus
        {
            get { return m_current; }
            protected set { m_current = value; }
        }

        public Vector3 WorldPosition => transform.position;

        public override void Reveal()
        {
            m_current.Visibility = TacticalVisibility.Visible;
            m_anim.SetBool("hidden", false);
            OnReveal?.Invoke(this);
        }
        public override void Hide()
        {
            m_current.Visibility = TacticalVisibility.Hidden;
            m_anim.SetBool("hidden", true);
            OnHide?.Invoke(this);
        }

        public virtual void Affect(Func<TacticalStats, TacticalStats> effect)
        {
            if (effect == null) return;
            CurrentStatus = effect.Invoke(CurrentStatus);
        }
        public override void Recover(int val)
        {
            Debug.Log("Recover enemy");
        }
        public override void TakeHit(int hitVal)
        {
            Debug.Log("Ahh, me ded");
            m_anim?.SetTrigger("takeHit");
            Defeat();
        }
        public virtual void ExecuteAction()
        {
            ResetCountdown();
            Reveal();
            m_anim?.SetTrigger("strike");
        }

        public virtual bool UpdateCountdown(int dt)
        {
            if (m_targetsInSight.Count == 0) { return false; }
            StartCoroutine(CountdownTick(dt, Countdown));
            Countdown -= dt;
            bool countDownEnds = Countdown <= 0;
            return countDownEnds;
        }
        #endregion
        #region pooling implementation
        public override void Prepare()
        {
            // Get new id for enemy
            Id = new EnemyId(gameObject.name);
            m_current = m_initStats;
            Countdown = m_current.AttackCountdown;
            m_detect.OnDetected += OnDetectEnter;
            m_detect.OnExitDetection += OnDetectExit;
        }

        public override void ReturnToPool()
        {
            OnDefeat = null;
            m_detect.OnDetected -= OnDetectEnter;
            m_detect.OnExitDetection -= OnDetectExit;
            Origin?.Reclaim(this);
        }
        #endregion
        #region base class implementation
        protected virtual void OnDetect()
        {
            if (m_targetsInSight.Count > 0)
            {
                OnCombat();
            }
        }
        protected virtual void OnCombat()
        {
            ResetCountdown();
            m_anim?.SetBool("combat", true);
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
            StartCoroutine(HandleDefeat());
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
            #region Handlers calls
        IEnumerator HandleDefeat() 
        {
            m_anim?.SetBool("defeat", true);
            yield return new WaitUntil(
                () => 
                { 
                    return m_anim.GetCurrentAnimatorStateInfo(m_anim.GetLayerIndex("TakeHit")).IsName("Defeat") &&
                    m_anim.GetCurrentAnimatorStateInfo(m_anim.GetLayerIndex("TakeHit")).normalizedTime >= 1.0f; 
                });
            OnDefeat?.Invoke(this);
        }
        void OnDetectEnter(IPlayer adv)
        {
            m_targetsInSight.Add(adv);
            OnDetect();
        }
        void OnDetectExit(IPlayer adv)
        {
            if (m_targetsInSight.Remove(adv))
            {
                // Do some animation here
                Standby();
            }
        }
            #endregion
        #endregion
    }

}
