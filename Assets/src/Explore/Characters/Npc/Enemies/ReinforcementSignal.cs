using System.Collections;
using UnityEngine;
using Curry.Game;
using System;
using System.Collections.Generic;

namespace Curry.Explore
{
    public interface IStepOnTrigger
    {
        void Trigger(ICharacter overlapping);
    }
    [Serializable]
    public struct ReinforcementTarget
    {
        public int Countdown;
        public PoolableBehaviour ReinforcementUnit;
    }
    [Serializable]
    public struct ReinforcementList
    {
        [SerializeField] List<ReinforcementTarget> m_targets;
        public List<ReinforcementTarget> Targets => m_targets;
    }
    public class ReinforcementSignal : PoolableBehaviour, IEnemy, IStepOnTrigger
    {
        [SerializeField] protected Reinforcement m_spawn = default;
        int m_countdownTimer = 0;
        int m_countdown = 0;
        PoolableBehaviour m_spawnRef;
        public event OnEnemyUpdate OnDefeat;
        public event OnEnemyUpdate OnReveal;
        public event OnEnemyUpdate OnHide;
        public event OnMovementBlocked OnBlocked;
        public bool SpotsTarget => false;
        public EnemyId Id { get; protected set; }
        public IEnumerator BasicAction => OnSpawnReinforcement();
        public IEnumerator Reaction => OnSpawnReinforcement();
        public string Name => "Reinforcement";
        public int MaxHp => 1;
        public int CurrentHp => 1;
        public int MoveRange => 0;
        public int Speed => 0;
        public Vector3 WorldPosition => transform.position;
        public ObjectVisibility Visibility => ObjectVisibility.Visible;
        protected virtual bool CanSpawn => m_countdownTimer >= Countdown;
        public int Countdown { get { return m_countdown; } protected set { m_countdown = value; } }
        public PoolableBehaviour SpawnRef { get { return m_spawnRef; } protected set { m_spawnRef = value; } }


        public void Setup(ReinforcementTarget spawnTarget)
        {
            Countdown = spawnTarget.Countdown;
            SpawnRef = spawnTarget.ReinforcementUnit;
        }
        public override void Prepare()
        {
            m_countdownTimer = 0;
        }
        #region IEnemy calls
        public void OnDefeated()
        {
            ReturnToPool();
        }
        public void OnMovementBlocked(ICharacter blocking)
        {
        }
        public void Hide()
        {
            OnHide?.Invoke(this);
        }
        public void Move(Vector3 target)
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
        public bool Warp(Vector3 to)
        {
            return false;
        }
        #endregion

        public virtual bool OnAction(int dt, bool reaction, out IEnumerator action)
        {
            m_countdownTimer += dt;
            action = CanSpawn ? OnSpawnReinforcement() : null;
            return CanSpawn;
        }
        protected virtual IEnumerator OnSpawnReinforcement()
        {
            Spawn();
            yield return new WaitForSeconds(0.1f);
            OnDefeat?.Invoke(this);
        }
        protected virtual void Spawn()
        {
            if (SpawnRef == null)
            {
                Debug.LogWarning("reinforcement failed, spawn reference object is null.");
                return;
            }

            Debug.Log("Spawn reinforcement");
            m_countdownTimer = 0;
            m_spawn.ApplyEffect(transform.position, SpawnRef);
            
        }
        // When a character steps on this object before reinforcement arrives,
        // destrpy this signal (canceling the reinforcement).
        public void Trigger(ICharacter overlapping)
        {
            m_countdownTimer = 0;
            OnDefeat?.Invoke(this);
        }
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ICharacter character)) 
            {
                Trigger(character);
            }
        }

        public void ApplyModifier(IStatModifier<TacticalStats> mod)
        {
            return;
        }
    }
}
