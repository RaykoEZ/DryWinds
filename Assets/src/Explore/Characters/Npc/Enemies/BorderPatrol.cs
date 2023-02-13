using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    // A basic enemy with stealth level
    public class BorderPatrol : TacticalEnemy, IStealthy
    {
        [Range(1, 3)]
        [SerializeField] int m_stealthLevel = default;
        [SerializeField] DealDamageTo m_basicAttack = default;
        [SerializeField] CallBackup m_backup = default;
        public int StealthLevel => m_stealthLevel;
        public override void Prepare()
        {
            // Restore reinforcement uses
            m_backup.Refresh();
            base.Prepare();
        }
        protected override void OnDetect()
        {
            m_backup.Try(transform.position, out bool _);
        }
        protected override IEnumerator ExecuteAction_Internal()
        {
            foreach (IPlayer player in TargetsInSight) 
            {
                Reveal();
                m_anim?.SetTrigger("strike");
                m_basicAttack.ApplyEffect(player, this);
                yield break;
            }
        }
    }
}
