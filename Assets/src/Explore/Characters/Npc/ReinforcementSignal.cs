using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    public interface IStepOnTrigger
    {
        void Trigger(ICharacter overlapping);
    }
    public class ReinforcementSignal : PoolableBehaviour, IEnemy, IStepOnTrigger
    {
        [SerializeField] public LayerMask m_blocksSpawn = default;
        [SerializeField] protected int m_spawnCountdown = default;
        [SerializeField] protected Reinforcement m_spawn = default;
        int m_currentTimer = 0;
        public EnemyId Id { get; protected set; }

        public TacticalStats InitStatus => new TacticalStats();

        public TacticalStats CurrentStatus => new TacticalStats();

        public IEnumerator BasicAction => OnSpawn();

        public IEnumerator Reaction => OnSpawn();

        public string Name => "Reinforcement";

        public int MaxHp => 1;

        public int CurrentHp => 1;

        public Vector3 WorldPosition => transform.position;

        public ObjectVisibility Visibility => ObjectVisibility.Visible;
        protected virtual bool CanSpawn => m_currentTimer >= m_spawnCountdown;
        public event OnEnemyUpdate OnDefeat;
        public event OnEnemyUpdate OnReveal;
        public event OnEnemyUpdate OnHide;
        public event OnMovementBlocked OnBlocked;
        public bool OnUpdate(int dt)
        {
            m_currentTimer += dt;
            return CanSpawn;
        }
        public override void Prepare()
        {
            m_currentTimer = 0;
        }
        #region IEnemy calls
        public void OnDefeated()
        {
            ReturnToPool();
        }
        public void Hide()
        {
            OnHide?.Invoke(this);
        }
        public void Move(Vector2Int direction)
        {
            OnBlocked?.Invoke(transform.position);
        }
        public void Recover(int val)
        {
        }
        public void Reveal()
        {
            OnReveal?.Invoke(this);
        }
        public void TakeHit(int hitVal)
        {
        }
        #endregion
        protected virtual IEnumerator OnSpawn()
        {
            Spawn();
            yield return new WaitForSeconds(0.1f);
            OnDefeat?.Invoke(this);
        }
        protected virtual void Spawn()
        {
            // check if any characters are on top of spawner, do not spawn if true
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 0.1f, m_blocksSpawn);
            if (hit.Length == 0) 
            {
                Debug.Log("Spawn reinforcement");
                m_currentTimer = 0;
                m_spawn.ApplyEffect(transform.position);
            }
        }
        // When a character steps on this object before reinforcement arrives,
        // destrpy this signal (canceling the reinforcement).
        public void Trigger(ICharacter overlapping)
        {
            m_currentTimer = 0;
            OnDefeat?.Invoke(this);
        }
    }
}
