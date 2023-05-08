using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public class SweepScanner : AdventCard, ICooldown
    {
        public override IEnumerator ActivateEffect(ICharacter user)
        {
            Reveal reveal = (m_resources as Reveal_CardResource).RevealModule;
            reveal?.ApplyEffect(user, user);
            yield return new WaitForEndOfFrame();
        }
    }
}
