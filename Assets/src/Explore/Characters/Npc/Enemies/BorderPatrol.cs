using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    // A basic enemy with stealth level
    public class BorderPatrol : TacticalEnemy
    {
        [SerializeField] StandardStrike m_standardAttack = default;
        public override IReadOnlyList<AbilityContent> AbilityDetails => new List<AbilityContent> 
        {
            m_standardAttack.AbilityDetail
        };
        public override void Prepare()
        {
            base.Prepare();
            m_vfxHandler.SetupAsset(m_standardAttack.Vfx, m_standardAttack.VfxTimeline);
        }
        protected override EnemyIntent UpdateIntent()
        {
            return SpotsTarget? new EnemyIntent(m_standardAttack.AbilityDetail, ExecuteAction_Internal()) :
                EnemyIntent.None;
        }
        protected override IEnumerator ExecuteAction_Internal()
        {
            foreach (IPlayer player in TargetsInSight) 
            {
                m_vfxHandler.SetupAsset(m_standardAttack.Vfx, m_standardAttack.VfxTimeline);
                yield return base.ExecuteAction_Internal();
                m_standardAttack.Activate(player);
                yield return m_vfxHandler.PlaySequence();
                yield break;
            }
        }
    }
}
