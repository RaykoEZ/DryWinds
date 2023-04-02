using Curry.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnPlayerDetected(IPlayer player);
    public delegate void OnEnemyDetected(IEnemy enemy);
    public class CharacterDetector : MonoBehaviour
    {
        HashSet<IPlayer> m_targetsInSight = new HashSet<IPlayer>();
        HashSet<IEnemy> m_enemies = new HashSet<IEnemy>();
        public event OnPlayerDetected OnPlayerEnterDetection;
        public event OnPlayerDetected OnPlayerExitDetection;
        public event OnEnemyDetected OnEnemyEnterDetection;
        public event OnEnemyDetected OnEnemyExitDetection;
        public IReadOnlyCollection<IPlayer> TargetsInSight => m_targetsInSight;
        public IReadOnlyCollection<IEnemy> Enemies => m_enemies;
        public void Shutdown() 
        {
            m_targetsInSight.Clear();
            m_enemies.Clear();
        }

        void OnOverlap(Collider2D collider) 
        {
            if (collider.attachedRigidbody == null) 
            { 
                return;  
            }

            if (collider.attachedRigidbody.TryGetComponent(out IPlayer character))
            {
                m_targetsInSight.Add(character);
                OnPlayerEnterDetection?.Invoke(character);
            }
            else if (collider.attachedRigidbody.TryGetComponent(out IEnemy enemy))
            {
                m_enemies.Add(enemy);
                OnEnemyEnterDetection?.Invoke(enemy);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D c)
        {
            OnOverlap(c);
        }
        protected virtual void OnTriggerExit2D(Collider2D c)
        {
            if (c.TryGetComponent(out IPlayer character))
            {
                m_targetsInSight.Remove(character);
                OnPlayerExitDetection?.Invoke(character);
            }
            else if (c.TryGetComponent(out IEnemy enemy))
            {
                m_enemies.Remove(enemy);
                OnEnemyExitDetection?.Invoke(enemy);
            }
        }
    }
}
