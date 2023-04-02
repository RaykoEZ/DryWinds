using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void FireWeapon();
    public class SharpShooter : TacticalEnemy
    {
        [SerializeField] protected StormMarrowRound m_stormAmmo = default;
        [SerializeField] protected PositionTargetingModule m_deadEyeCheck = default;
        bool m_deadEye = false;
        protected event FireWeapon Fire;
        protected void FiringWeapon() 
        {
            Fire?.Invoke();
        }
        protected override bool ChooseAction_Internal(int dt, out IEnumerator action)
        {
            // if we see target, do basic action
            if (SpotsTarget)
            {
                // Check for dead Eye
                m_deadEye = DeadEyeCheck();
                action = ExecuteAction_Internal();
            }
            else
            {
                // find all enemies who can see a target
                action = Disengage();
            }
            return SpotsTarget;
        }
        internal IEnumerator Disengage() 
        {
            Hide();
            yield return null;
        }
        protected override IEnumerator ExecuteAction_Internal()
        {
            // Listen to fore trigger in animation
            Fire += OnFireWeapon;
            yield return base.ExecuteAction_Internal();
            yield return new WaitUntil(() => Fire == null);
        }
        internal void OnFireWeapon() 
        {
            StartCoroutine(FireWeapon_Internal());
        }
        // Fires weapon when animation triggers event
        protected internal virtual IEnumerator FireWeapon_Internal() 
        {
            Fire -= OnFireWeapon;
            foreach (IPlayer target in TargetsInSight)
            {
                Vector3 dir = target.WorldPosition - transform.position;
                Quaternion rot = Quaternion.LookRotation(dir, Vector3.forward);
                StormMarrowRound instance = Instantiate(m_stormAmmo, transform.position, rot, transform.parent);
                // OnAttack, if target in deadEye range, upgrade attack instance
                if (m_deadEye) 
                {
                    Debug.Log("Dead Eye activate");
                    instance.Upgrade();
                }
                yield return StartCoroutine(instance.FireAt(target.WorldPosition));
                break;
            }
            yield return null;
        }

        protected virtual bool DeadEyeCheck() 
        {
            return m_deadEyeCheck.HasValidTarget(transform.position, out List<IPlayer> _);
        }
    }
}
