using System.Collections;

namespace Curry.Explore
{

    public class Healing : AdventCard, IConsumable
    {
        // Card Effect
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            HealingModule heal = (m_resources as Heal_CardResource).Healing;
            heal?.ApplyEffect(user, user);
            yield return null;
        }
        public IEnumerator OnExpend()
        {
            yield return null;
        }
    }
}
