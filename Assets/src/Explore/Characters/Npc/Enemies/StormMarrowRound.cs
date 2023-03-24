using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Curry.Explore
{
    public class StormMarrowRound : MonoBehaviour, IProjectile
    {
        [SerializeField] protected DealDamageTo m_damage = default;
        [SerializeField] protected Animation m_anim = default;
        [SerializeField] AnimationClip m_onHit = default;
        public List<ITargetEffectModule> OnHitEffect => new List<ITargetEffectModule> { m_damage };

        public void Deflect(Vector3 deflectDirection)
        {
            Stop();
        }

        public virtual IEnumerator FireAt(Vector3 targetPos)
        {
            yield return null;
        }
        public void Stop()
        {
            Destroy(gameObject);
        }
        protected virtual void OnTriggerEnter2D(Collider2D col) 
        { 
            if (col.gameObject.TryGetComponent(out ICharacter hit)) 
            {
                OnHit(hit);
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
