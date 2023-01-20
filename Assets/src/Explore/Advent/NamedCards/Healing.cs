using System.Collections;
using UnityEngine;

namespace Curry.Explore
{

    public class Healing : AdventCard 
    {
        [SerializeField] HealingModule m_healing = default;
        // Card Effect
        public override IEnumerator ActivateEffect(IPlayer user)
        {
            m_healing.ApplyEffect(user, user);
            yield return null;
            OnExpend();
        }
    }
}
