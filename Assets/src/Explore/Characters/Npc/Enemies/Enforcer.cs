using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public class Enforcer : TacticalEnemy 
    {
        [SerializeField] StandardStrike m_basicAttack = default;
        [SerializeField] ReliefAlly m_relief = default;
        public override IReadOnlyList<AbilityContent> AbilityDetails => 
            new List<AbilityContent>{
                m_basicAttack.AbilityDetail,
                m_relief.AbilityDetail
            };
        protected override EnemyIntent UpdateIntent(ActionCost dt)
        {
            EnemyIntent ret;
            if (SpotsTarget) 
            {
                ret = new EnemyIntent(m_basicAttack.AbilityDetail, ExecuteAction_Internal());
            }
            else 
            {
                ReliefCheck(out EnemyIntent result);
                ret = result;
            }
            return ret;
        }
        protected bool ReliefCheck(out EnemyIntent action) 
        {
            bool reliefNeeded = false;
            action = null;
            // find all enemies who can see a target
            List<IEnemy> activeEnemies = new List<IEnemy>();
            foreach (IEnemy e in EnemiesInSight)
            {
                if (e.SpotsTarget)
                {
                    activeEnemies.Add(e);
                }
            }
            // set basic action, if we have nearby enemies who sees enemies,
            // swap position with one of them
            if (activeEnemies.Count > 0)
            {
                int rand = UnityEngine.Random.Range(0, activeEnemies.Count);
                action = new EnemyIntent(m_relief.AbilityDetail, ReliefAlly(activeEnemies[rand]));
                reliefNeeded = true;
            }
            return reliefNeeded && action != null;
        }
        protected override bool UpdatAction_Internal(int dt, out EnemyIntent reaction)
        {
            return ReliefCheck(out reaction);
        }

        protected override IEnumerator ExecuteAction_Internal()
        {
            yield return base.ExecuteAction_Internal();
            foreach (IPlayer player in TargetsInSight)
            {
                m_basicAttack.Activate(player);
                break;
            }
            yield return null;
        }

        protected virtual IEnumerator ReliefAlly(ICharacter target) 
        {
            m_relief?.Activate(target, this);
            yield return new WaitForEndOfFrame();
        }
    }
}
