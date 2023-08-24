using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Healing : CardResource, IConsumable
    {
        [SerializeField] Heal_EffectResource m_healing = default;
        public Healing(Healing effect) : base(effect)
        {
            m_healing = effect.m_healing;
        }
        // Card Effect
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            Action onTrigger = () =>
            {
                m_healing?.Healing?.ApplyEffect(user);
            };
            var vfx = user.AddVfx(m_vfx, m_vfxTimeLine);
            yield return vfx?.PlayerVfx(onTrigger);
            yield return new WaitForSeconds(0.1f);
            vfx?.StopVfx();
        }
        public IEnumerator OnExpend()
        {
            yield return null;
        }
    }
}
