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
            m_standardAttack.Content
        };
        protected override bool ChooseAction_Internal(int dt, out IEnumerator action)
        {
            bool ret = base.ChooseAction_Internal(dt, out action);
            return ret;
        }
        protected override AbilityContent ShowIntent()
        {
            return SpotsTarget? m_standardAttack.Content: AbilityContent.None;
        }
        protected override IEnumerator ExecuteAction_Internal()
        {
            foreach (IPlayer player in TargetsInSight) 
            {
                yield return base.ExecuteAction_Internal();
                m_standardAttack.Activate(player);
                yield break;
            }
        }
    }
}
