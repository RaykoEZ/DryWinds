using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public class Enforcer : TacticalEnemy 
    {
        [SerializeField] StandardStrike m_basicAttack = default;
        public override IReadOnlyList<AbilityContent> AbilityDetails => 
            new List<AbilityContent>{
                m_basicAttack.AbilityDetail,
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
                ret = EnemyIntent.None;
            }
            return ret;
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
    }
}
