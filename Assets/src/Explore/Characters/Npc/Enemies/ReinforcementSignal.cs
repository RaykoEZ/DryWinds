﻿using System.Collections;
using UnityEngine;
using Curry.Game;
using System;
using System.Collections.Generic;
using TMPro;
using Curry.Util;
using UnityEngine.VFX;
using UnityEngine.Timeline;

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
        [SerializeField] protected TextMeshPro m_countdownDisplay = default;
        int m_countdownTimer = 0;
        int m_countdown = 0;
        protected Reinforcement m_spawn = default;
        PoolableBehaviour m_spawnRef;
        public event OnCharacterUpdate OnDefeat;
        public event OnCharacterUpdate OnReveal;
        public event OnCharacterUpdate OnHide;
        public event OnHpUpdate TakeDamage;
        public event OnHpUpdate RecoverHp;
        public event OnCharacterUpdate OnMoveFinished;
        public event OnMovementBlocked OnBlocked;
        public event OnAbilityMessage OnAbility;
        public bool SpotsTarget => false;
        public EnemyId Id { get { return new EnemyId(gameObject.name); } }
        public IEnumerator BasicAction => OnSpawnReinforcement();
        public IEnumerator Reaction => OnSpawnReinforcement();
        public string Name => "Reinforcement";
        public int MaxHp => 1;
        public int CurrentHp { get { return 1; } set { } }
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
                TargetingRange = default,
                Icon = default
            } 
        };
        public EnemyIntent IntendingAction => EnemyIntent.None;

        public void Setup(Reinforcement_EffectResource reinforce)
        {
            m_spawn = reinforce.ReinforcementModule;
            ReinforcementList pool = reinforce.TargetPool;
            ReinforcementTarget target = SamplingUtil.SampleFromList(pool.Targets, 1)[0];
            CountdownDuration = target.Countdown;
            SpawnRef = target.ReinforcementUnit;
            m_countdownDisplay.text = CountdownTimer.ToString();
        }
        public override void Prepare()
        {
            m_countdownTimer = 0;
        }
        public virtual bool UpdateAction(ActionCost dt, out EnemyIntent action)
        {
            m_countdownTimer += dt.Time;
            action = CanSpawn ? new EnemyIntent(AbilityDetails[0], OnSpawnReinforcement()) : null;
            m_countdownDisplay.text = CountdownTimer.ToString();
            return CanSpawn;
        }
        protected virtual IEnumerator OnSpawnReinforcement()
        {
            OnAbility?.Invoke(AbilityDetails[0].Name);
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
            StartCoroutine(OnDefeated());
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
        public void Move(Vector3 target)
        {
            OnMoveFinished?.Invoke(this);
            OnBlocked?.Invoke(transform.position);
        }
        public bool Warp(Vector3 to)
        {
            return false;
        }

        public Transform GetTransform()
        {
            return transform;
        }
        public IEnumerator OnDefeated()
        {
            OnDefeat?.Invoke(this);
            yield return null;
        }

        public void TriggerVfx(VisualEffectAsset vfx, TimelineAsset timeline, Action onTrigger = null)
        {
        }
    }
}
