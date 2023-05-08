using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public class Enforcer : TacticalEnemy 
    {
        [SerializeField] StandardStrike m_basicAttack = default;
        [SerializeField] ReliefAlly m_relief = default;
        public override IReadOnlyList<AbilityContent> AbilityDetails => 
            new List<AbilityContent>{
                m_basicAttack.                Content,
                m_relief.                Content
            };
        protected override bool ChooseAction_Internal(int dt, out IEnumerator action)
        {
            bool reliefNeeded = false;
            // if we see target, do basic action
            if (SpotsTarget) 
            {
                action = ExecuteAction_Internal();
            }
            else 
            {
                // find all enemies who can see a target
                reliefNeeded = ReliefCheck(out action);                
            }
            return SpotsTarget || reliefNeeded;
        }
        protected bool ReliefCheck(out IEnumerator action) 
        {
            bool reliefNeeded = false;
            action = null;
            // find all enemies who can see a target
            List<IEnemy> activeEnemies = new List<IEnemy>();
            foreach (IEnemy e in EnemiesInSight)
            {
                if (e.SpotsTarget)
                {
                    activeEnemies.Add(e);
                }
            }
            // set basic action, if we have nearby enemies who sees enemies,
            // swap position with one of them
            if (activeEnemies.Count > 0)
            {
                int rand = Random.Range(0, activeEnemies.Count);
                action = ReliefAlly(activeEnemies[rand]);
                reliefNeeded = true;
            }
            return reliefNeeded && action != null;
        }
        protected override bool ChooseReaction_Internal(int dt, out IEnumerator reaction)
        {
            return ReliefCheck(out reaction);
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

        protected virtual IEnumerator ReliefAlly(ICharacter target) 
        {
            m_relief?.Activate(target, this);
            yield return new WaitForEndOfFrame();
        }
    }
}
