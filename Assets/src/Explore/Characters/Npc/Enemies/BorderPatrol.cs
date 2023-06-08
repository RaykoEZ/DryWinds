using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    // A basic enemy with stealth level
    public class BorderPatrol : TacticalEnemy
    {
        [SerializeField] StandardStrike m_standardAttack = default;
        [SerializeField] CallBackup m_backup = default;
        public override IReadOnlyList<AbilityContent> AbilityDetails => new List<AbilityContent> 
        {
            m_standardAttack.Content,
            m_backup.Content
        };
        public override void Prepare()
        {
            // Restore reinforcement uses
            m_backup.Refresh();
            base.Prepare();
        }
        protected override void OnDetect()
        {
            m_backup.Try(transform.position, out bool _);
            base.OnDetect();
        }
        protected override IEnumerator ExecuteAction_Internal()
        {
            foreach (IPlayer player in TargetsInSight) 
            {
                m_anim?.SetTrigger("strike");
                m_standardAttack.Activate(player);
                yield break;
            }
        }
    }
}
