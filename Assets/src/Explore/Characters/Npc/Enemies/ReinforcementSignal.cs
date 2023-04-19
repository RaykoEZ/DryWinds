using System.Collections;
using UnityEngine;
using Curry.Game;
using System;
using System.Collections.Generic;
using TMPro;

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
        [SerializeField] protected TextMeshPro m_countdownDisplay = default;
        int m_countdownTimer = 0;
        int m_countdown = 0;
        PoolableBehaviour m_spawnRef;
        public event OnCharacterUpdate OnDefeat;
        public event OnCharacterUpdate OnReveal;
        public event OnCharacterUpdate OnHide;
        public event OnHpUpdate TakeDamage;
        public event OnHpUpdate RecoverHp;
        public event OnMovementBlocked OnBlocked;

        public bool SpotsTarget => false;
        public EnemyId Id { get { return new EnemyId(gameObject.name); } }
        public IEnumerator BasicAction => OnSpawnReinforcement();
        public IEnumerator Reaction => OnSpawnReinforcement();
        public string Name => "Reinforcement";
        public int MaxHp => 1;
        public int CurrentHp => 1;
        public int MoveRange => 0;
        public int Speed => 0;
        public Vector3 WorldPosition => transform.position;
        public ObjectVisibility Visibility => ObjectVisibility.Visible;
        protected virtual bool CanSpawn => m_countdownTimer >= CountdownDuration;
        public int CountdownTimer => CountdownDuration - m_countdownTimer;
        public int CountdownDuration { get { return m_countdown; } protected set { m_countdown = value; } }
        public PoolableBehaviour SpawnRef { get { return m_spawnRef; } protected set { m_spawnRef = value; } }
        public IReadOnlyList<AbilityContent> AbilityDetails => new List<AbilityContent> 
        { 
            new AbilityContent { 
                Name = "Reinforcement", 
                Description = 
                $"Reinforce inbound in: {CountdownTimer} dt. (Occupy this position in time to stop this!)",
                RangePattern = default,
                Icon = default
            } 
        };
        public void Setup(ReinforcementTarget spawnTarget)
        {
            CountdownDuration = spawnTarget.Countdown;
            SpawnRef = spawnTarget.ReinforcementUnit;
            m_countdownDisplay.text = CountdownTimer.ToString();
        }
        public override void Prepare()
        {
            m_countdownTimer = 0;
        }
        public virtual bool OnAction(int dt, bool reaction, out IEnumerator action)
        {
            m_countdownTimer += dt;
            action = CanSpawn ? OnSpawnReinforcement() : null;
            m_countdownDisplay.text = CountdownTimer.ToString();
            return CanSpawn;
        }
        protected virtual IEnumerator OnSpawnReinforcement()
        {
            Spawn();
            yield return new WaitForSeconds(0.1f);
            OnDefeated();
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
            OnDefeated();
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
        public void Despawn()
        {
            m_countdownTimer = 0;
            m_countdownDisplay.text = "";
            ReturnToPool();
        }

        public void Reveal()
        {
            OnReveal?.Invoke(this);
        }

        public void Hide()
        {
            OnHide?.Invoke(this);
        }

        public void Recover(int val)
        {
            RecoverHp?.Invoke(val, CurrentHp);
        }

        public void TakeHit(int hitVal)
        {
            TakeDamage?.Invoke(hitVal, CurrentHp);
        }

        public void OnDefeated()
        {
            OnDefeat?.Invoke(this);
        }

        public void Move(Vector3 target)
        {
            OnBlocked?.Invoke(WorldPosition);
        }
        public bool Warp(Vector3 to)
        {
            return false;
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}
