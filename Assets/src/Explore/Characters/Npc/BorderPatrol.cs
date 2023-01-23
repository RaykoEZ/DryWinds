using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Curry.Util;

namespace Curry.Explore
{
    // A basic enemy with stealth level
    public class BorderPatrol : TacticalEnemy, IStealthy
    {
        [Range(1, 3)]
        [SerializeField] int m_stealthLevel = default;
        [SerializeField] DealDamageTo m_basicAttack = default;
        [SerializeField] Reinforcement m_reinforcement = default;
        public int StealthLevel => m_stealthLevel;

        protected override void OnDetect()
        {
            CallReinforcement();
        }
        protected virtual void CallReinforcement() 
        {
            RangeMap adjacentOffset = RangeMap.Adjacent;
            List<Vector3> spawnPositions = adjacentOffset.ApplyRangeOffsets(transform.position);
            List<Vector3> unoccupied = new List<Vector3>();
            foreach (Vector3 p in spawnPositions)
            {
                Collider2D hit = Physics2D.OverlapCircle(p, 0.1f, m_reinforcement.DoNotSpawnOn);
                if (!hit)
                {
                    unoccupied.Add(p);
                }
            }

            if (unoccupied.Count > 0) 
            {
                int rand = UnityEngine.Random.Range(0, unoccupied.Count);
                m_reinforcement.ApplyEffect(unoccupied[rand]);
            }
        }
        protected override IEnumerator ExecuteAction_Internal()
        {
            yield return base.ExecuteAction_Internal();
            Reveal();
            foreach (IPlayer player in m_targetsInSight) 
            {
                m_basicAttack.ApplyEffect(player, this);
                break;
            }
            yield return null;
        }
    }
}
