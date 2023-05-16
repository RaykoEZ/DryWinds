using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public class Foresight : AdventCard, ICooldown
    {
        [SerializeField] Reveal_EffectResource m_reveal = default;
        public override IEnumerator ActivateEffect(ICharacter target)
        {
            m_reveal?.RevealModule?.ApplyEffect(target);
            yield return new WaitForEndOfFrame();
        }
    }
}
