using System.Collections;
using UnityEngine;
using Curry.Events;
using UnityEngine.InputSystem;

namespace Curry.Explore
{
    #region some game event argument definitions
    public class ScanInfo : EventInfo 
    {
        public int DetectionLevel { get; protected set; }
        public float Diameter { get; protected set; }
        public Vector3 OriginWorldPosition { get; protected set; }

        public ScanInfo(int detectionLevel, float diameter, Vector3 origin)
        {
            DetectionLevel = detectionLevel;
            Diameter = diameter;
            OriginWorldPosition = origin;
        }
    }
    #endregion
    // A basic player character for adventure mode
    public class Adventurer : TacticalCharacter, IPlayer
    {
        #region Serialize Fields
        [SerializeField] Animator m_anim = default;
        // Pings player status to trigger card draw events etc
        [SerializeField] CurryGameEventTrigger m_onPlayerPing = default;
        [SerializeField] CurryGameEventTrigger m_onScout = default;
        #endregion
        IRescue m_rescuee;
        #region IPlayer interface impl
        public event OnTakeDamage TakeDamage;
        public event OnPlayerUpdate OnDefeat;
        public event OnPlayerUpdate OnReveal;
        public event OnPlayerUpdate OnHide;
        public event OnPlayerUpdate OnMoveFinished;
        public override void Reveal()
        {
            OnReveal?.Invoke(this);
        }
        public override void Hide()
        {
            OnHide?.Invoke(this);
        }
        public override void Recover(int val)
        {
            base.Recover(val);
            TakeDamage?.Invoke(val, CurrentHp);
        }
        public override void TakeHit(int hitVal)
        {
            Debug.Log("Player takes" + hitVal + " damage.");
            m_anim.ResetTrigger("TakeDamage");
            m_anim.SetTrigger("TakeDamage");
            CurrentHp -= hitVal;
            TakeDamage?.Invoke(hitVal, CurrentHp);

            if (CurrentHp <= 0) 
            {
                OnDefeated();
            }
        }

        public override void OnDefeated()
        {
            Debug.Log("Player defeated");
            OnDefeat?.Invoke(this);
        }
        #endregion

        #region Movement impl
        protected override void OnMoveFinish()
        {
            CharacterInfo info = new CharacterInfo(this);
            m_onPlayerPing?.TriggerEvent(info);
            m_onScout?.TriggerEvent(info);
            if (m_rescuee != null)
            {
                m_rescuee.Rescue();
            }
            OnMoveFinished?.Invoke(this);
        }
        #endregion
        void Start()
        {
            // Ping once at start
            CharacterInfo info = new CharacterInfo(this);
            m_onScout?.TriggerEvent(info);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.TryGetComponent(out IRescue rescue))
            {
                m_rescuee = rescue;
            }
        }
    }
}
