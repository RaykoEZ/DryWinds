using Curry.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    // A modifier to decrease movement range 
    public class Impediment : TacticalModifier, IMovementElement<TacticalStats>, ITargetEffectModule
    {
        [SerializeField] int m_numBeforeExpiry = default;
        [SerializeField] int m_rangeDecrease = default;
        public Impediment(Impediment source) 
        {
            m_name = source.m_name;
            m_numBeforeExpiry = source.m_numBeforeExpiry;
            m_rangeDecrease = source.m_rangeDecrease;
        }

        public void ApplyEffect(ICharacter target)
        {
            target.ApplyModifier(this);
        }

        public virtual void OnCharacterMoved(TacticalStats stat)
        {
            m_numBeforeExpiry--;
            if( m_numBeforeExpiry <= 0) 
            {
                Expire();
            }
        }
        protected override TacticalStats Apply_Internal(TacticalStats baseVal)
        {
            baseVal.MoveRange -= m_rangeDecrease;
            return baseVal;
        }
    }

    public class StormMarrowRound : MonoBehaviour, IProjectile
    {
        [SerializeField] protected DealDamageTo m_damage = default;
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] protected AnimationClip m_onImpact = default;
        [SerializeField] protected Impediment m_impedeEffect = default;
        protected List<ITargetEffectModule> m_onHit = new List<ITargetEffectModule> {};
        public IReadOnlyList<ITargetEffectModule> OnHitEffects => m_onHit;
        protected bool upgraded = false;
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
