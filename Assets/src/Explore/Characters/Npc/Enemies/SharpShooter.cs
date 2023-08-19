using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public interface IRangedUnit 
    {
        IProjectile RangedAttack { get; }
    }
    public delegate void FireWeapon();
    public class SharpShooter : TacticalEnemy, IRangedUnit
    {
        [SerializeField] protected StormMarrowRound m_stormAmmo;
        [SerializeField] protected Deadeye m_deadEye;
        [SerializeField] protected PatientHunter m_patient;
        StormMarrowRound m_currentProjectileInstance;
        protected event FireWeapon Fire;
        protected override List<IEnemyReaction> m_reactions => new List<IEnemyReaction> 
        { 
            m_patient
        };
        public override IReadOnlyList<AbilityContent> AbilityDetails => new List<AbilityContent> 
        {
            m_stormAmmo.AbilityDetail,
            m_deadEye.AbilityDetail,
            m_patient.AbilityDetail
        };
        public IProjectile RangedAttack => m_currentProjectileInstance;
        protected void FiringWeapon() 
        {
            Fire?.Invoke();
        }
        protected override EnemyIntent UpdateIntent()
        {
            return SpotsTarget ? new EnemyIntent(m_stormAmmo.AbilityDetail, ExecuteAction_Internal()) : 
                EnemyIntent.None;
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
                m_currentProjectileInstance = Instantiate(m_stormAmmo, transform.position, rot, transform.parent);
                m_currentProjectileInstance.Setup(this);
                // Check for Dead Eye activation condition, activate if we can activate
                if (m_deadEye.TryActivate<IPlayer>(this, out _)) 
                {
                    StartCoroutine(m_vfxHandler.PlaySequence());
                }
                yield return StartCoroutine(m_currentProjectileInstance.FireAt(target.WorldPosition));
                break;
            }
            yield return null;
        }
    }
}
