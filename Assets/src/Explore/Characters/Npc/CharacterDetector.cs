using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnPlayerDetected(IPlayer player);
    public delegate void OnEnemyDetected(IEnemy enemy);
    public class CharacterDetector : MonoBehaviour
    {
        List<IPlayer> m_targetsInSight = new List<IPlayer>();
        List<IEnemy> m_enemies = new List<IEnemy>();
        public event OnPlayerDetected OnPlayerEnterDetection;
        public event OnPlayerDetected OnPlayerExitDetection;

        public event OnEnemyDetected OnEnemyEnterDetection;
        public event OnEnemyDetected OnEnemyExitDetection;
        public IReadOnlyList<IPlayer> TargetsInSight => m_targetsInSight;
        public IReadOnlyList<IEnemy> Enemies => m_enemies;
        protected virtual void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.TryGetComponent(out IPlayer character))
            {
                m_targetsInSight.Add(character);
                OnPlayerEnterDetection?.Invoke(character);
            }
            else if (c.gameObject.TryGetComponent(out IEnemy enemy)) 
            {
                m_enemies.Add(enemy);
                OnEnemyEnterDetection?.Invoke(enemy);
            }
        }
        protected virtual void OnTriggerExit2D(Collider2D c)
        {
            if (c.gameObject.TryGetComponent(out IPlayer character))
            {
                m_targetsInSight.Remove(character);
                OnPlayerExitDetection?.Invoke(character);
            }
            else if (c.gameObject.TryGetComponent(out IEnemy enemy))
            {
                m_enemies.Remove(enemy);
                OnEnemyExitDetection?.Invoke(enemy);
            }
        }
    }
}
