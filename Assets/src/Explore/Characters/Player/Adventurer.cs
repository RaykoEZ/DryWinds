using System.Collections;
using UnityEngine;
using Curry.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Curry.Explore
{
    public interface IMovementLimiter 
    {
        bool CanMove { get; }
        int CurrentMoveCount { get; }
        int MaxMoveCount { get; }
        void UpdateMoveLimit(int change = 1);
    }

    // A basic player character for adventure mode
    public class Adventurer : TacticalCharacter, IPlayer, IMovementLimiter
    {
        #region Serialize Fields
        [SerializeField] Animator m_anim = default;
        [SerializeField] MovementCount m_moveCount = default;
        // Pings player status to trigger card draw events etc
        [SerializeField] CurryGameEventTrigger m_onPlayerPing = default;
        [SerializeField] CurryGameEventTrigger m_onScout = default;
        #endregion
        public bool CanMove => m_moveCount.Current > 0;
        public override IReadOnlyList<AbilityContent> AbilityDetails => new List<AbilityContent>();
        public int CurrentMoveCount => m_moveCount.Current;
        public int MaxMoveCount => m_moveCount.Max;
        #region IPlayer interface impl
        public override void Prepare()
        {
            m_moveCount.Init();
            base.Prepare();
        }

        protected override void TakeHit_Internal(int hitVal)
        {
            Debug.Log("Player takes " + hitVal + " damage.");
            m_anim.ResetTrigger("TakeDamage");
            m_anim.SetTrigger("TakeDamage");
        }
        public override void OnDefeated()
        {
            Debug.Log("Player defeated");
            base.OnDefeated();
        }
        #endregion

        #region Movement impl
        protected override void OnMoveFinish()
        {
            base.OnMoveFinish();
            CharacterInfo info = new CharacterInfo(this);
            m_onPlayerPing?.TriggerEvent(info);
            m_onScout?.TriggerEvent(info);
        }
        #endregion
        void Start()
        {
            // Ping once at start
            CharacterInfo info = new CharacterInfo(this);
            m_onScout?.TriggerEvent(info);
        }
        public void UpdateMoveLimit(int change = 1)
        {
            m_moveCount.UpdateCount(change);
        }
    }
}
