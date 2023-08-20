using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public interface IRangedUnit 
    {
        IProjectile RangedAttack { get; }

        IEnumerator FireWeapon();
    }
    public delegate void FireWeapon();
    public class SharpShooter : TacticalEnemy, IRangedUnit
    {
        [SerializeField] protected StormMarrowRound m_stormAmmo = default;
        [SerializeField] protected Deadeye m_deadEye = default;
        [SerializeField] protected PatientHunter m_patient = default;
        StormMarrowRound m_currentProjectileInstance;
        protected event FireWeapon Fire;
        public override IReadOnlyList<AbilityContent> AbilityDetails => new List<AbilityContent> 
        {
            m_stormAmmo.AbilityDetail,
            m_deadEye.AbilityDetail,
            m_patient.AbilityDetail
        };
        public IProjectile RangedAttack => m_currentProjectileInstance;
        public override void Prepare()
        {
            base.Prepare();
            OnTargetMoved += FireAtMovingTarget;
        }
        public override void ReturnToPool()
        {
            OnTargetMoved -= FireAtMovingTarget;
            base.ReturnToPool();
        }
        protected void FiringWeapon() 
        {
            Fire?.Invoke();
        }
        protected override EnemyIntent UpdateIntent()
        {
            return m_patient.CanReact(this) ? new EnemyIntent(m_patient.AbilityDetail, Enhance()) : 
                EnemyIntent.None;
        }
        // Trigger PatientHunter stack
        protected IEnumerator Enhance()
        {
            // Listen to fore trigger in animation
            yield return m_patient?.OnPlayerAction(this);
        }
        void FireAtMovingTarget(ICharacter target)
        {
            StartCoroutine(FireWeapon());
        }
        public IEnumerator FireWeapon()
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
                m_deadEye.TryActivate<IPlayer>(this, out _);
                yield return StartCoroutine(m_currentProjectileInstance.FireAt(target.WorldPosition));
                break;
            }
            yield return null;
        }
    }
}
