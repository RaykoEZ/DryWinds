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
        [SerializeField] protected PlayerDetector m_detect = default;
        protected HashSet<IPlayer> m_targetsInSight = new HashSet<IPlayer>();
        protected TacticalStats m_current;

        #region ICharacter & IEnemy interface 

        public event OnEnemyUpdate OnDefeat;
        public event OnEnemyUpdate OnReveal;
        public event OnEnemyUpdate OnHide;

        public virtual EnemyId Id { get; protected set; }
        public TacticalStats InitStatus { get { return m_initStats; } }
        public override ObjectVisibility Visibility { get { return CurrentStatus.Visibility; }
            protected set { m_current.Visibility = value; }
        }
        public TacticalStats CurrentStatus
        {
            get { return m_current; }
            protected set { m_current = value; }
        }
        public IEnumerator BasicAction => ExecuteAction_Internal();

        public IEnumerator Reaction => Reaction_Internal();

        public override void Reveal()
        {
            m_current.Visibility = ObjectVisibility.Visible;
            m_anim.SetBool("hidden", false);
            OnReveal?.Invoke(this);
        }
        public override void Hide()
        {
            m_current.Visibility = ObjectVisibility.Hidden;
            m_anim.SetBool("hidden", true);
            OnHide?.Invoke(this);
        }
        public override void Recover(int val)
        {
            Debug.Log("Recover enemy");
        }
        public override void TakeHit(int hitVal)
        {
            m_anim?.SetTrigger("takeHit");
            Defeat();
        }
        protected virtual IEnumerator ExecuteAction_Internal()
        {
            Reveal();
            m_anim?.SetTrigger("strike"); 
            yield return null;
        }
        protected virtual IEnumerator Reaction_Internal() 
        {
            Debug.Log("Standby");
            yield return new WaitForSeconds(0.1f);
        }

        public virtual bool OnUpdate(int dt)
        {
            return m_targetsInSight.Count > 0;
        }
        #endregion

        #region pooling implementation
        public override void Prepare()
        {
            // Get new id for enemy
            Id = new EnemyId(gameObject.name);
            m_current = m_initStats;
            m_detect.OnDetected += OnDetectEnter;
            m_detect.OnExitDetection += OnDetectExit;
        }
        public override void ReturnToPool()
        {
            OnDefeat = null;
            m_detect.OnDetected -= OnDetectEnter;
            m_detect.OnExitDetection -= OnDetectExit;
            base.ReturnToPool();
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
            m_anim?.SetBool("combat", true);
        }
        protected virtual void Standby()
        {
            //reset anim state and countdown
            m_anim?.SetBool("combat", false);
        }
        protected virtual void Defeat()
        {
            StartCoroutine(HandleDefeat());
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
