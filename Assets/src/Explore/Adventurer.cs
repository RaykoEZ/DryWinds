using System.Collections;
using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    #region some game event argument definitions
    public class ScanInfo : EventInfo 
    {
        public int DetectionLevel { get; protected set; }
        public float Diameter { get; protected set; }
        public ScanInfo(int detectionLevel, float diameter)
        {
            DetectionLevel = detectionLevel;
            Diameter = diameter;
        }
    }
    public class PlayerInfo : EventInfo
    {
        public AdventurerStats PlayerStats { get; protected set; }
        public PlayerInfo(AdventurerStats stats) 
        {
            PlayerStats = stats;
        }
    }
    #endregion
    // A basic player character for adventure mode
    public class Adventurer : MonoBehaviour, IPlayer
    {
        #region Serialize Fields
        [SerializeField] Animator m_anim = default;
        [SerializeField] AdventurerStats m_startingStats = default;
        [SerializeField] CurryGameEventListener m_onMove = default;
        // Pings player status to trigger card draw events etc
        [SerializeField] CurryGameEventTrigger m_onPlayerPing = default;
        [SerializeField] CurryGameEventTrigger m_onScout = default;
        [SerializeField] CurryGameEventTrigger m_moveFinish = default;
        #endregion
        IRescue m_rescuee;

        #region IPlayer interface impl
        public event OnTakeDamage TakeDamage;
        public event OnPlayerUpdate OnDefeat;
        public event OnPlayerUpdate OnReveal;
        public event OnPlayerUpdate OnHide;
        protected AdventurerStats m_current;
        public AdventurerStats StartingStats { get { return new AdventurerStats(m_startingStats); } }
        public AdventurerStats CurrentStats { get { return new AdventurerStats(m_current); } }

        public virtual void Reveal()
        {
            OnReveal?.Invoke(this);
            Debug.Log("Reveal player"); 
        }
        public virtual void Hide()
        {
            OnHide?.Invoke(this);
            Debug.Log("Hide player");
        }
        public virtual void Recover(int val)
        {
            Debug.Log("Player recovers" + val + " HP.");
            val = Mathf.Clamp(val, 0, m_startingStats.HP - m_current.HP);
            m_current.HP += val;
            TakeDamage?.Invoke(val, m_current.HP);
        }
        public virtual void TakeHit(int hitVal)
        {
            Debug.Log("Player takes" + hitVal + " damage.");
            m_anim.ResetTrigger("TakeDamage");
            m_anim.SetTrigger("TakeDamage");
            m_current.HP -= hitVal;
            TakeDamage?.Invoke(hitVal, m_current.HP);

            if (m_current.HP <= 0) 
            {
                OnDefeated();
            }
        }

        public virtual void OnDefeated()
        {
            Debug.Log("Player defeated");
            OnDefeat?.Invoke(this);
        }
        public virtual void Move(Vector2Int direction)
        {
            Vector3 target = transform.position + new Vector3(direction.x, direction.y, 0f);
            StartCoroutine(Move_Internal(target));
        }
        #endregion

        #region Movement impl
        public void Move(EventInfo info) 
        {
            if (info == null) return;

            Vector3 target;
            // Depending on event param type, set target target position 
            if (info is PositionInfo select) 
            {
                target = select.WorldPosition;
                target.z = transform.position.z;
                StartCoroutine(Move_Internal(target));
            }
        }
        IEnumerator Move_Internal(Vector3 target)
        {
            RaycastHit2D hit = Physics2D.Linecast(
                    transform.position,
                    target,
                    LayerMask.GetMask("Obstacles"));
            // Check for walls
            if (hit)
            {
                yield break;
            }

            float duration = 1f;
            float timeElapsed = 0f;
            while (timeElapsed <= duration)
            {
                timeElapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, target, timeElapsed / duration);
                yield return null;
            }
            PlayerInfo info = new PlayerInfo(new AdventurerStats(m_startingStats));
            m_moveFinish?.TriggerEvent(info);
            m_onPlayerPing?.TriggerEvent(info);
            m_onScout?.TriggerEvent(info);

            if (m_rescuee != null)
            {
                m_rescuee.Rescue();
            }
        }
        #endregion
        void Awake()
        {
            m_current = new AdventurerStats(StartingStats);
            m_onMove?.Init();
        }
        void Start()
        {
            // Ping once at start
            PlayerInfo info = new PlayerInfo(new AdventurerStats(CurrentStats));
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
