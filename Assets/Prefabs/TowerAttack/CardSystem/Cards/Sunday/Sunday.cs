using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Sunday : CardResource, IConsumable
    {
        [SerializeField] GainAp_EffectResource m_actionGain = default;
        public Sunday(Sunday effect) : base(effect)
        {
            m_actionGain = effect.m_actionGain;
        }
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {
            Action onTrigger = () => {
                m_actionGain?.Activate(context);
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