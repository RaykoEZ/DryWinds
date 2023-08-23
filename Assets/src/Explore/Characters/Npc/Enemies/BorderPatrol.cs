using System.Collections.Generic;
using UnityEngine;
using Curry.Vfx;
namespace Curry.Explore
{
    // A basic enemy with stealth level
    public class BorderPatrol : TacticalEnemy
    {
        [SerializeField] StandardStrike m_standardAttack = default;
        VfxManager.VfxHandle m_slash;
        public override IReadOnlyList<AbilityContent> AbilityDetails => new List<AbilityContent> 
        {
            m_standardAttack.AbilityDetail
        };
        public override void Prepare()
        {
            base.Prepare();
            m_slash = m_vfx.AddVfx(m_standardAttack.Vfx, m_standardAttack.VfxTimeline);
        }
        public override void ReturnToPool()
        {
            m_slash?.StopVfx();
            base.ReturnToPool();
        }
        protected override EnemyIntent UpdateIntent()
        {
            return SpotsTarget? new EnemyIntent(m_standardAttack.AbilityDetail, ExecuteAction_Internal()) :
                EnemyIntent.None;
        }
        public void Strike()
        {
            StartCoroutine(m_slash?.PlayerVfx(OnImpact));
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
