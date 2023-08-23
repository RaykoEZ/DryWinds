using System.Collections;
using UnityEngine;
using Curry.Events;
using System.Collections.Generic;

namespace Curry.Explore
{
    // A basic player character for adventure mode
    public class Adventurer : TacticalCharacter, IPlayer
    {
        #region Serialize Fields
        [SerializeField] Animator m_anim = default;
        // Pings player status to trigger card draw events etc
        [SerializeField] CurryGameEventTrigger m_onPlayerPing = default;
        [SerializeField] CurryGameEventTrigger m_onScout = default;
        #endregion
        public override IReadOnlyList<AbilityContent> AbilityDetails => new List<AbilityContent>();
        #region IPlayer interface impl
        protected override void TakeHit_Internal(int hitVal)
        {
            m_anim.ResetTrigger("takeHit");
            m_anim.SetTrigger("takeHit");
        }
        public override IEnumerator OnDefeated()
        {
            yield return StartCoroutine(OnDefeat_Internal());
            yield return base.OnDefeated();
        }
        IEnumerator OnDefeat_Internal() 
        {
            m_anim.SetBool("defeat", true);
            yield return new WaitForSeconds(1f);
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
    }
}
