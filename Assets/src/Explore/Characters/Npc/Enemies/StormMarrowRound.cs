using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public class StormMarrowRound : MonoBehaviour, IProjectile
    {
        [SerializeField] protected DealDamageTo m_damage = default;
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] protected AnimationClip m_onImpact = default;
        public List<ITargetEffectModule> OnHitEffect => new List<ITargetEffectModule> { m_damage };

        public void Deflect(Vector3 deflectDirection)
        {
            Stop();
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
            foreach (ITargetEffectModule module in OnHitEffect)  
            {
                module?.ApplyEffect(hit);
            }
        }
    }
}
