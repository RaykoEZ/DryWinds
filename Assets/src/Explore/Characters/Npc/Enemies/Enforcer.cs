using System.Collections.Generic;
using UnityEngine;
using Curry.Vfx;

namespace Curry.Explore
{
    public class Enforcer : TacticalEnemy 
    {
        [SerializeField] StandardStrike m_basicAttack = default;
        public override IReadOnlyList<AbilityContent> AbilityDetails => 
            new List<AbilityContent>{
                m_basicAttack.AbilityDetail,
            };
        VfxManager.VfxHandle m_slash;
        public override void Prepare()
        {
            base.Prepare();
            m_slash = m_vfx.AddVfx(m_basicAttack.Vfx, m_basicAttack.VfxTimeline);
        }
        public override void ReturnToPool()
        {
            m_slash?.StopVfx();
            base.ReturnToPool();
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
        public void Strike() 
        {
            StartCoroutine(m_slash?.PlayerVfx(OnImpact));
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
