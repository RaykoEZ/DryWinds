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
        [SerializeField] protected CharacterDetector m_detect = default;
        protected TacticalStats m_current;
        protected IReadOnlyList<IPlayer> TargetsInSight => m_detect.TargetsInSight;
        protected IReadOnlyList<IEnemy> EnemiesInSight => m_detect.Enemies;

        #region ICharacter & IEnemy interface 
        public event OnEnemyUpdate OnDefeat;
        public event OnEnemyUpdate OnReveal;
        public event OnEnemyUpdate OnHide;
        public bool SpotsTarget => TargetsInSight.Count > 0;
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
        // Actions & Reactions for the unit, initialized in Prepare() on spawn.
        public virtual IEnumerator BasicAction { get; protected set; }

        public virtual IEnumerator Reaction { get; protected set; }
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
            Debug.Log("reaction blockout");
            yield return new WaitForEndOfFrame();
        }

        // returns true if we decide to act,
        // BasicAction & Reaction fields need to not be null before returning
        public virtual bool ChooseAction(int dt)
        {
            // Setting default action and reaction methods
            BasicAction = ExecuteAction_Internal();
            Reaction = Reaction_Internal();
            return SpotsTarget;
        }
        #endregion

        #region pooling implementation
        public override void Prepare()
        {
            // Setting default action and reaction methods
            BasicAction = ExecuteAction_Internal();
            Reaction = Reaction_Internal();
            // Get new id for enemy
            Id = new EnemyId(gameObject.name);
            m_current = m_initStats;
            m_detect.OnPlayerEnterDetection += OnDetectEnter;
            m_detect.OnPlayerExitDetection += OnDetectExit;
            m_detect.OnEnemyEnterDetection += OnOtherEnemyEnter;
            m_detect.OnEnemyExitDetection += OnOtherEnemyExit;
        }
        public override void ReturnToPool()
        {
            OnDefeat = null;
            m_detect.OnPlayerEnterDetection -= OnDetectEnter;
            m_detect.OnPlayerExitDetection -= OnDetectExit;
            m_detect.OnEnemyEnterDetection -= OnOtherEnemyEnter;
            m_detect.OnEnemyExitDetection -= OnOtherEnemyExit;
            base.ReturnToPool();
        }
        #endregion

        #region base class implementation
        protected virtual void OnDetect()
        {
            if (TargetsInSight.Count > 0)
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
            m_anim.SetBool("isInActive", false);
            StartCoroutine(HandleDefeat());
        }
        #endregion

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
            OnDetect();
        }
        void OnDetectExit(IPlayer adv)
        {
            // Do some animation here
            Standby();
        }
        protected virtual void OnOtherEnemyEnter(IEnemy other) { }
        protected virtual void OnOtherEnemyExit(IEnemy other) { }

        #endregion
    }

}
