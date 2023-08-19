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
        public override void Prepare()
        {
            base.Prepare();
            m_vfxHandler.SetupAsset(m_basicAttack.Vfx, m_basicAttack.VfxTimeline);
        }
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
        protected override IEnumerator ExecuteAction_Internal()
        {
            yield return base.ExecuteAction_Internal();
            foreach (IPlayer player in TargetsInSight)
            {
                m_vfxHandler.SetupAsset(m_basicAttack.Vfx, m_basicAttack.VfxTimeline);
                yield return m_vfxHandler.PlaySequence();
                m_basicAttack.Activate(player);
                break;
            }
            yield return null;
        }
    }
}
