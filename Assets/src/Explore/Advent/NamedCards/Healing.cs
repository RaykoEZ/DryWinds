using System.Collections;
using UnityEngine;

namespace Curry.Explore
{

    public class Healing : AdventCard, IConsumable
    {
        [SerializeField] HealingModule m_healing = default;
        [SerializeField] ConsumableModule m_consume = default;
        public int MaxUses => m_consume.MaxUses;

        public int UsesLeft => m_consume.UsesLeft;

        // Card Effect
        public override IEnumerator ActivateEffect(IPlayer user)
        {
            m_healing.ApplyEffect(user, user);
            yield return null;
        }

        public IEnumerator OnExpend()
        {
            return m_consume.OnExpend();
        }

        public void OnUse()
        {
            m_consume.OnUse();
        }

        public void SetMaxUses(int newMax)
        {
            m_consume.SetMaxUses(newMax);
        }

        public void SetUsesLeft(int newUsesLeft)
        {
            m_consume.SetUsesLeft(newUsesLeft);
        }
    }
}
