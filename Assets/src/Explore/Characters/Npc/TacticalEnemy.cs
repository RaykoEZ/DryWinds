using Curry.Game;
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
        protected IReadOnlyCollection<IPlayer> TargetsInSight => m_detect.TargetsInSight;
        protected IReadOnlyCollection<IEnemy> EnemiesInSight => m_detect.Enemies;

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

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ICharacter block)) 
            {
                block?.OnMovementBlocked(this);
            }
        }
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
        // returns true if we decide to act,
        // BasicAction & Reaction fields need to not be null before returning
        public bool OnAction(int dt, bool reaction, out IEnumerator action)
        {
            bool ret;
            if (reaction) 
            {
                ret = ChooseReaction_Internal(dt, out IEnumerator result);
                action = result;
            } 
            else 
            {
                ret = ChooseAction_Internal(dt, out IEnumerator result);
                action = result;
            }
            return ret;
        }
        protected virtual bool ChooseAction_Internal(int dt, out IEnumerator action) 
        {
            action = ExecuteAction_Internal();
            return SpotsTarget;
        }
        protected virtual bool ChooseReaction_Internal(int dt, out IEnumerator reaction) 
        {
            reaction = Reaction_Internal();
            return false;
        }
        protected virtual IEnumerator ExecuteAction_Internal()
        {
            Reveal();
            m_anim?.SetTrigger("strike");
            yield return null;
        }
        protected virtual IEnumerator Reaction_Internal()
        {
            yield return new WaitForEndOfFrame();
        }
        #endregion

        #region pooling implementation
        public override void Prepare()
        {
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
            m_detect.Shutdown();
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
