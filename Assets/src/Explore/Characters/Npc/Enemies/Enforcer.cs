﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public class Enforcer : TacticalEnemy 
    {
        [SerializeField] DealDamageTo m_basicAttack = default;
        [SerializeField] SwapPosition m_relief = default;
        protected override bool ChooseAction_Internal(int dt, out IEnumerator action)
        {
            bool reliefNeeded = false;
            action = null;
            // if we see target, do basic action
            if (SpotsTarget) 
            {
                action = ExecuteAction_Internal();
            }
            else 
            {
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
            }
            return SpotsTarget || reliefNeeded;
        }

        protected override IEnumerator ExecuteAction_Internal()
        {
            yield return base.ExecuteAction_Internal();
            Reveal();
            foreach (IPlayer player in TargetsInSight)
            {
                m_basicAttack.ApplyEffect(player, this);
                break;
            }
            yield return null;
        }

        protected virtual IEnumerator ReliefAlly(ICharacter target) 
        {
            m_relief?.ApplyEffect(target, this);
            yield return new WaitForEndOfFrame();
        }
    }
}
