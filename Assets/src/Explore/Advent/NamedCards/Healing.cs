using UnityEngine;

namespace Curry.Explore
{

    public class Healing : AdventCard 
    {
        [SerializeField] HealingModule m_healing = default;
        // Card Effect
        protected override void ActivateEffect(IPlayer user)
        {
            m_healing.ApplyEffect(user, user);
            OnExpend();
        }
    }
}
