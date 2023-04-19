using Curry.Game;
using Curry.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{

    public class StormMarrowRound : BaseAbility, IProjectile, IAbility, IDamageAbility
    {
        [SerializeField] protected DealDamageTo m_damage = default;
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] protected AnimationClip m_onImpact = default;
        [SerializeField] protected Impediment m_impedeEffect = default;
        [SerializeField] protected RangeMapDatabase m_rangedb = default;
        [Range(1, 5)]
        [SerializeField] protected int m_rangeRadius = default;
        protected List<ITargetEffectModule> m_onHit = new List<ITargetEffectModule> {};
        public int Damage => m_damage.AddDamage + m_damage.BaseDamage;
        public IReadOnlyList<ITargetEffectModule> OnHitEffects => m_onHit;

        public override RangeMap Range => m_rangedb.GetSquareRadiusMap(m_rangeRadius);

        protected bool upgraded = false;
        public override AbilityContent GetContent()
        {
            var ret = base.GetContent();
            ret.Name = "StormMarrow Strike";
            ret.Description = $"Deal {Damage} (+{m_damage.AddDamage}) damage to target.";   
            return ret;
        }
        public void AddOnHitEffect(ITargetEffectModule mod)
        {
            if (mod == null) return;

            m_onHit.Add(mod);
        }

        public void AddDamage(int val)
        {
            m_damage.AddDamage += val;
        }
        public void Deflect(Vector3 deflectDirection)
        {
            Stop();
        }
        // Upgrade attack
        public void Upgrade()
        {
            if (upgraded) return;
            upgraded = true;
            m_damage.AddDamage += 1;
            Impediment effect = new Impediment(m_impedeEffect);
            m_onHit.Add(effect);
        }
        public virtual IEnumerator FireAt(Vector3 targetPos)
        {
            float duration = 0.1f;
            float timeElapsed = 0f;
            while (timeElapsed <= duration)
            {
                timeElapsed += Time.deltaTime;
                transform.position = Vector2.Lerp(transform.position, targetPos, timeElapsed / duration);
                yield return new WaitForEndOfFrame();
            }
            transform.rotation = Quaternion.identity;
            m_anim?.Play(m_onImpact.name);
            yield return new WaitForSeconds(0.25f * m_onImpact.length);
            OnImpact();
            Stop();
        }
        public void Stop()
        {
            Destroy(gameObject);
        }
        // When projectile lands, loonk for characters to hit on position
        protected virtual void OnImpact() 
        {
            var hit = Physics2D.OverlapCircleAll(
                transform.position, 
                0.49f,
                LayerMask.GetMask(MovementManager.s_gameplayCollisionFilters));
            foreach (Collider2D item in hit)
            {
                if(item.TryGetComponent(out ICharacter character)) 
                {
                    OnHit(character);
                }
            }        
        }
        protected virtual void OnHit(ICharacter hit) 
        {
            m_onHit.Add(m_damage);
            foreach (ITargetEffectModule module in OnHitEffects)  
            {
                module?.ApplyEffect(hit);
            }
        }
    }
}
