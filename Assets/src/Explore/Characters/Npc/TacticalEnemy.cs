using Curry.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    // Base enemy class
    public abstract class TacticalEnemy : TacticalCharacter, IEnemy, IMovableEnemy, IPoolable
    {
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] protected CharacterDetector m_detect = default;
        public event OnEnemyMove OnMove;
        protected IReadOnlyCollection<IPlayer> TargetsInSight => m_detect.TargetsInSight;
        protected IReadOnlyCollection<IEnemy> EnemiesInSight => m_detect.Enemies;
        protected virtual List<IEnemyReaction> m_reactions { get; } = 
            new List<IEnemyReaction>();
        #region ICharacter & IEnemy interface 
        public bool SpotsTarget => TargetsInSight.Count > 0;
        public virtual EnemyId Id { get; protected set; }
        public override void Move(Vector3 target)
        {
            OnMove?.Invoke(this, target, base.Move);
        }
        public override void Reveal()
        {
            m_anim.SetBool("hidden", false);
            base.Reveal();
        }
        public override void Hide()
        {
            m_anim.SetBool("hidden", true);
            base.Hide();
        }
        public override void Recover(int val)
        {
            Debug.Log("Recover enemy");
        }
        protected override void TakeHit_Internal(int hitVal)
        {
            m_anim?.SetTrigger("takeHit");
        }
        // returns true if we decide to act,
        // BasicAction & Reaction fields need to not be null before returning
        public bool OnAction(ActionCost dt, bool reaction, out IEnumerator action)
        {
            bool ret;
            if (reaction) 
            {
                ret = ChooseReaction_Internal(dt.Time, out IEnumerator result);
                action = result;
            } 
            else 
            {
                ret = ChooseAction_Internal(dt.Time, out IEnumerator result);
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
            return m_reactions.Count > 0;
        }
        protected virtual IEnumerator ExecuteAction_Internal()
        {
            CurrentStats.Refresh();
            // Update any modifier changes before action
            m_anim?.SetTrigger("strike");
            yield return null;
        }
        protected virtual IEnumerator Reaction_Internal()
        {
            CurrentStats.Refresh();
            foreach (IEnemyReaction onAction in m_reactions )
            {
                onAction?.OnPlayerAction(this);
            }
            yield return new WaitForEndOfFrame();
        }
        #endregion

        #region pooling implementation
        public override void Prepare()
        {
            base.Prepare();
            // Get new id for enemy
            Id = new EnemyId(gameObject.name);
            m_detect.OnPlayerEnterDetection += OnDetectEnter;
            m_detect.OnPlayerExitDetection += OnDetectExit;
            m_detect.OnEnemyEnterDetection += OnOtherEnemyEnter;
            m_detect.OnEnemyExitDetection += OnOtherEnemyExit;
        }
        public override void ReturnToPool()
        {
            m_detect.Shutdown();
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
                Reveal();
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
            OnDefeated();
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
