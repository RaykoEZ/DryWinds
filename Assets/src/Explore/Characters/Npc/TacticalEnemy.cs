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
        [SerializeField] protected Animator m_detectTrigger = default;
        [SerializeField] protected CharacterDetector m_detect = default;
        public event OnEnemyMove OnMove;
        public event OnAbilityMessage OnAbility;
        protected IReadOnlyCollection<IPlayer> TargetsInSight => m_detect.TargetsInSight;
        protected IReadOnlyCollection<IEnemy> EnemiesInSight => m_detect.Enemies;
        protected virtual List<IEnemyReaction> m_reactions { get; } = 
            new List<IEnemyReaction>();
        EnemyIntent m_intendingAction = EnemyIntent.None;
        #region ICharacter & IEnemy interface 
        public bool SpotsTarget => TargetsInSight.Count > 0;
        public virtual EnemyId Id { get; protected set; }
        public EnemyIntent IntendingAction => m_intendingAction;
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
        public bool UpdateAction(ActionCost dt, out EnemyIntent action)
        {
            bool ret;
            // Setup action to carry out after player end turn
            var newIntent = UpdateIntent(dt);
            ret = UpdatAction_Internal(dt.Time, out EnemyIntent result);
            m_intendingAction = newIntent == null ? EnemyIntent.None : newIntent;
            action = result == null? EnemyIntent.None : result;
            return ret;
        }
        protected virtual bool UpdatAction_Internal(int dt, out EnemyIntent reaction) 
        {
            reaction = new EnemyIntent(AbilityContent.None, Reaction_Internal());
            bool ret = false;
            // Check if reaction is active
            foreach (var item in m_reactions)
            {
                ret |= item.CanReact(this);
            }
            return ret;
        }
        protected virtual IEnumerator ExecuteAction_Internal()
        {
            CurrentStats.Refresh();
            // Update any modifier changes before action
            m_anim?.SetTrigger("strike");
            // reset pending ability
            yield return null;
        }
        protected virtual IEnumerator Reaction_Internal()
        {
            CurrentStats.Refresh();
            foreach (IEnemyReaction onAction in m_reactions )
            {
                yield return onAction?.OnPlayerAction(this);
                yield return new WaitForSeconds(0.5f);
            }
            // reset pending ability
            yield return new WaitForEndOfFrame();
        }
        protected virtual EnemyIntent UpdateIntent(ActionCost dt)
        {
            return EnemyIntent.None;
        }
        #endregion
        protected virtual void OnAbilityMessageTrigger(string message) 
        {
            OnAbility?.Invoke(message);
        }
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
            foreach (var item in m_reactions)
            {
                item.OnMessage += OnAbilityMessageTrigger;
            }
        }
        public override void ReturnToPool()
        {
            m_detect.Shutdown();
            m_detect.OnPlayerEnterDetection -= OnDetectEnter;
            m_detect.OnPlayerExitDetection -= OnDetectExit;
            m_detect.OnEnemyEnterDetection -= OnOtherEnemyEnter;
            m_detect.OnEnemyExitDetection -= OnOtherEnemyExit;
            foreach (var item in m_reactions)
            {
                item.OnMessage -= OnAbilityMessageTrigger;
            }
            base.ReturnToPool();
        }
        #endregion

        #region base class implementation
        protected virtual void OnDetect()
        {
            if (TargetsInSight.Count > 0)
            {
                m_audio?.Play("detect");
                m_detectTrigger?.ResetTrigger("detectTrigger");
                m_detectTrigger?.SetTrigger("detectTrigger");
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
        public override IEnumerator OnDefeated()
        {
            yield return StartCoroutine(HandleDefeat());
            yield return base.OnDefeated();
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
