using Curry.Game;
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
        protected override EnemyIntent UpdateIntent()
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
        public void Strike() 
        {
            m_vfxHandler.SetupAsset(m_basicAttack.Vfx, m_basicAttack.VfxTimeline);
            StartCoroutine(m_vfxHandler.PlayVfxSequence(OnImpact));
        }
        void OnImpact() 
        {
            foreach (IPlayer player in TargetsInSight)
            {
                m_basicAttack.Activate(player);
                break;
            }
        }
    }
}
