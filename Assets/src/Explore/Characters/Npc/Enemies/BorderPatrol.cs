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
        protected override EnemyIntent UpdateIntent()
        {
            return SpotsTarget? new EnemyIntent(m_standardAttack.AbilityDetail, ExecuteAction_Internal()) :
                EnemyIntent.None;
        }
        public void Strike()
        {
            m_vfxHandler.SetupAsset(m_standardAttack.Vfx, m_standardAttack.VfxTimeline);
            StartCoroutine(m_vfxHandler.PlayVfxSequence(OnImpact));
        }
        void OnImpact()
        {
            foreach (IPlayer player in TargetsInSight)
            {
                m_standardAttack.Activate(player);
                break;
            }
        }
    }
}
