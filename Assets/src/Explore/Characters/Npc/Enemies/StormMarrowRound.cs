using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Curry.Explore
{
    public class StormMarrowRound : MonoBehaviour, IProjectile
    {
        [SerializeField] protected DealDamageTo m_damage = default;
        public List<ITargetEffectModule> OnHitEffect => new List<ITargetEffectModule> { m_damage };

        public void Deflect(Vector3 deflectDirection)
        {
            Stop();
        }

        public void Fire(Vector3 targetPos)
        {
            StartCoroutine(Fire_Internal(targetPos));
        }
        protected virtual IEnumerator Fire_Internal(Vector3 targetPos) 
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
