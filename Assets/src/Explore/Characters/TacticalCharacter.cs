﻿using Curry.Game;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.AI;

namespace Curry.Explore
{
    public abstract class TacticalCharacter : PoolableBehaviour, ICharacter
    {
        [SerializeField] protected string m_name = default;
        [SerializeField] TacticalStats m_initStats = default;
        protected TacticalStatManager m_statManager;
        protected bool m_blocked = false;
        protected bool m_moving = false;
        public Vector3 WorldPosition => transform.position;
        public string Name => m_name;
        public int MaxHp => m_statManager.Current.MaxHp;
        public int CurrentHp => m_statManager.Current.Hp;      
        public int MoveRange => m_statManager.Current.MoveRange;     
        public int Speed => m_statManager.Current.Speed;
        public virtual ObjectVisibility Visibility => m_statManager.Current.Visibility;

        public event OnMovementBlocked OnBlocked;
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_moving && collision.TryGetComponent(out ICharacter block))
            {
                OnMovementBlocked(block);
            }
        }
        public override void Prepare()
        {
            m_statManager = new TacticalStatManager();
            m_statManager.Init(m_initStats);
        }
        public abstract void Hide();
        public virtual void Move(Vector3 target)
        {
            StartCoroutine(Move_Internal(target));
        }
        protected virtual void OnMovementBlocked(ICharacter blocking) 
        {
            Debug.Log("blocked by: " + blocking.Name);
            if (blocking.Equals(this)) 
            {
                return;
            }
            m_blocked = true;
            Reveal();
            blocking.Reveal();
        }

        public virtual void OnDefeated()
        {
            ReturnToPool();
        }
        public virtual void Recover(int val)
        {
            Debug.Log("Player recovers " + val + " HP.");
            m_statManager.RecoverHp(val);
        }
        public abstract void Reveal();
        public void TakeHit(int hitVal) 
        {
            int result = m_statManager.CalculateDamage(hitVal);
            m_statManager.TakeDamage(result);
            TakeHit_Internal(result);

            if (CurrentHp <= 0)
            {
                OnDefeated();
            }
        }
        protected abstract void TakeHit_Internal(int hitVal);
        protected virtual IEnumerator Move_Internal(Vector3 target)
        {
            m_blocked = false;
            m_moving = true;
            yield return new WaitForEndOfFrame();
            float duration = 1f;
            float timeElapsed = 0f;
            while (timeElapsed <= duration)
            {
                if (m_blocked)
                {
                    OnBlocked?.Invoke(WorldPosition);
                    break;
                }
                timeElapsed += Time.deltaTime;
                transform.position = Vector2.Lerp(transform.position, target, timeElapsed / duration);
                yield return null;
            }
            OnMoveFinish();
        }
        protected virtual void OnMoveFinish() 
        {
            m_moving = false;
            m_statManager.OnMovementFinish();
        }
        public bool Warp(Vector3 to)
        {
            int boundFilter = 1 << LayerMask.NameToLayer("Environment");
            Collider2D[] hit = Physics2D.OverlapCircleAll(
                to,
                0.1f, layerMask: boundFilter);
            // only warp to positions with terrain tiles (inside our level's bounds)
            if (hit.Length > 0) 
            {
                transform.position = to;
            }
            return hit.Length > 0;
        }
        public void ApplyModifier(IStatModifier<TacticalStats> mod)
        {
            m_statManager.AddModifier(mod);
        }
        public void OnTimeElapsed(int dt) 
        {
            m_statManager.OnTimeElapsed(dt);
        }
    }

}
